using Azure.Core;
using BlogIT.Data;
using BlogIT.Data.Models;
using BlogIT.DataTransferObjects;
using BlogIT.Interfaces;
using BlogIT.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public AuthenticationController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> Login ([FromBody]UserLoginDto userLoginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userLoginDto.Username);

            if(user == null)
            {
                return Unauthorized("Invalid username");
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);

            if(!loginResult.Succeeded)
            {
                return Unauthorized("Incorrect username or password.");
            }

            string token = _tokenService.GenerateToken(user);

            var existingRefreshToken = await _context.RefreshTokens
             .Where(r => r.UserId == user.Id)
             .ToListAsync(); ;

            if (existingRefreshToken.Any())
            {
                _context.RefreshTokens.RemoveRange(existingRefreshToken);
            }

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = _tokenService.GenerateRefreshToken(),
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token
            });
        }


        [HttpPost()]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var tokenEntry = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (tokenEntry is null || tokenEntry.ExpiresOn < DateTime.UtcNow)
            {
                return Unauthorized("The refresh token has expired");
            }

            var user = tokenEntry.User;

            _context.RefreshTokens.Remove(tokenEntry);
           
            string token = _tokenService.GenerateToken(tokenEntry.User);

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = _tokenService.GenerateRefreshToken(),
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = token,
                RefreshToken = newRefreshToken.Token
            });
        }
       
        [HttpPut]
        public async Task SeedAsync()
        {
            // Check if any users exist in the database, if not, add test users
            var existingUser = await _userManager.FindByEmailAsync("testuser@example.com");
            if (existingUser != null)
            {
                var user = new User
                {
                    UserName = "testusesr",
                    Email = "testusersss@example.com",
                    Name = "Test",
                    LastName = "User",
                    BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-25)), // Example birthdate
                    RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
                };

                var result = await _userManager.CreateAsync(user, "TestPassword123!");

                if (result.Succeeded)
                {
                    // Optionally, assign roles here
                    // await _userManager.AddToRoleAsync(user, "Admin");

                    Console.WriteLine("Test user created successfully.");
                }
                else
                {
                    Console.WriteLine("Error creating test user.");
                }
            }
        }
    }
}

