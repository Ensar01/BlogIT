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
            _context = _context;
            _logger = logger;
        }

        public async Task<bool> UserExists(string username, string phoneNumber, string email)
        {
            
            var userByUsername = await _userManager.FindByNameAsync(username);
            if (userByUsername != null)
            {
                return true; 
            }
           
            var userByEmail = await _userManager.FindByEmailAsync(email);
            if (userByEmail != null)
            {
                return true; 
            }

            var userByPhoneNumber = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (userByPhoneNumber != null)
            {
                return true; 
            }

            return false; 
        }
      
        public async Task<IdentityResult> RegisterUser(UserRegisterDto userRegisterDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            var user = new User
            {
                Name = userRegisterDto.Name,
                LastName = userRegisterDto.LastName,
                BirthDate = userRegisterDto.BirthDate,
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                PhoneNumber = userRegisterDto.PhoneNumber
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

            await transaction.CommitAsync();
            _logger.LogInformation("User {Email} successfully registered and role assigned.", userRegisterDto.Email);


            return IdentityResult.Success;
        }

      
    }
}
