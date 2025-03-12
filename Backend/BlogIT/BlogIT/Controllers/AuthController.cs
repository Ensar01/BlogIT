using Azure.Core;
using BlogIT.Data;
using BlogIT.Data.Models;
using BlogIT.DataTransferObjects;
using BlogIT.Interfaces;
using BlogIT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ITokenStorageService _tokenStorageService;
        private readonly AuthService _authService;
        private readonly UserService _userService;
        public AuthController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService,
            AuthService refreshTokenService, ITokenStorageService tokenStorageService, UserService userService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _authService = refreshTokenService;
            _tokenStorageService = tokenStorageService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _userService.UserExists(userRegisterDto.UserName, userRegisterDto.Email))
                {
                    return BadRequest(new ValidationProblemDetails
                    {
                        Title = "Validation Error",
                        Detail = "A user with this username or email already exists.",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                var result = await _userService.RegisterUser(userRegisterDto);

                if (result.Succeeded)
                {
                    return Ok("User registered successfully.");
                }


                return BadRequest(result.Errors);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Login ([FromBody]UserLoginDto userLoginDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userLoginDto.Username);
            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);

            if (user == null || !loginResult.Succeeded)
            {
                return Unauthorized("Invalid credentials");
            }

            var AuthTokenDto = await _authService.GenerateTokens(user);

            _tokenStorageService.SetTokens(AuthTokenDto);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            _tokenStorageService.RevokeCookies();
            return NoContent();
        }


        [HttpPost()]
        public async Task<IActionResult> RefreshToken()
        {
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            var tokenEntry = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (tokenEntry is null || tokenEntry.ExpiresOn < DateTime.UtcNow)
            {
                return Unauthorized("The refresh token has expired");
            }

            var user = tokenEntry.User;

            _context.RefreshTokens.Remove(tokenEntry);
            await _context.SaveChangesAsync();

            var AuthTokenDto = await _authService.GenerateTokens(user);

            _tokenStorageService.SetTokens(AuthTokenDto);

            return Ok();
        }
        [HttpGet]
        public async Task<bool> CheckAvailability (string? email, string?username)
        {
            if(!string.IsNullOrEmpty(email))
            {
                return await _context.User.AnyAsync(u => u.Email == email);
            }
            if (!string.IsNullOrEmpty(username))
            {
                return await _context.User.AnyAsync(u => u.UserName == username);
            }

            return false;
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

