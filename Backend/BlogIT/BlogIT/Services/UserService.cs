using BlogIT.Data;
using BlogIT.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public UserService(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = _context;
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
      
    }
}
