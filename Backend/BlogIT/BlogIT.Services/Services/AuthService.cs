using BlogIT.Data.Models;
using BlogIT.Data;
using BlogIT.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogIT.DataTransferObjects;
using BlogIT.Model.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using BlogIT.Interfaces.Interfaces;

namespace BlogIT.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenStorageService _tokenStorageService;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext context, UserManager<User> userManager, ILogger<AuthService> logger, SignInManager<User> signInManager,
            ITokenStorageService tokenStorageService, ITokenService tokenService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _tokenStorageService = tokenStorageService;
            _tokenService = tokenService;
        }
        public async Task<bool> UserExists(string email, string username)
        {

            return await _userManager.Users.AnyAsync(u =>
                 u.UserName == username || u.Email == email);

        }


        public async Task<IdentityResult> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var user = new User
            {
                Name = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var createUser = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!createUser.Succeeded)
            {
                _logger.LogError("User creation failed for {Email}: {Errors}", userRegisterDto.Email,
                    string.Join(", ", createUser.Errors.Select(e => e.Description)));
                return createUser;
            }

            _logger.LogInformation("User {Email} created successfully. Assigning role...", userRegisterDto.Email);
            var userRole = await _userManager.AddToRoleAsync(user, "User");

            if (!userRole.Succeeded)
            {
                _logger.LogError("Failed to assign role 'User' to {Email}: {Errors}", userRegisterDto.Email,
                    string.Join(", ", userRole.Errors.Select(e => e.Description)));
                await transaction.RollbackAsync();
                return IdentityResult.Failed(userRole.Errors.ToArray());
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation("User {Email} successfully registered and role assigned.", userRegisterDto.Email);


            return IdentityResult.Success;
        }
        public async Task<bool> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null)
                return false;

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return false;

            var userTokenDto = new UserTokenDto(user.Email, user.UserName, user.Id);
            var tokens = await _tokenService.GenerateTokens(userTokenDto);


            _tokenStorageService.SetTokens(tokens);

            return true;
        }
    }
}

