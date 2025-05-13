using BlogIT.Data;
using BlogIT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;
        public UserController(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
            
        }
        [HttpGet]
        public async Task<bool> Exists(string? email, string? username)
        {
            return await _authService.UserExists(email, username);
        }
    }
}
