using BlogIT.Data;
using BlogIT.Data.Models;
using BlogIT.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(UserManager<User> userManager, ApplicationDbContext context, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UserExists(string username, string email)
        {

            return await _userManager.Users.AnyAsync(u =>
                 u.UserName == username || u.Email == email );
        }
      
        public async Task<IdentityResult> RegisterUser(UserRegisterDto userRegisterDto)
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


      
    }
}
