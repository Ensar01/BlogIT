using Azure.Core;
using BlogIT.Data;
using BlogIT.Data.Models;
using BlogIT.DataTransferObjects;
using BlogIT.Interfaces;
using BlogIT.Interfaces.Interfaces;
using BlogIT.Model.DataTransferObjects;
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
        private readonly ITokenStorageService _tokenStorageService;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(ITokenStorageService tokenStorageService, IAuthService authService, ITokenService tokenService)
        {


            _tokenStorageService = tokenStorageService;
            _authService = authService;
            _tokenService = tokenService;
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
                if (await _authService.UserExists(userRegisterDto.Email, userRegisterDto.UserName))
                {
                    return BadRequest(new ValidationProblemDetails
                    {
                        Title = "Validation Error",
                        Detail = "A user with this username or email already exists.",
                        Status = StatusCodes.Status400BadRequest
                    });
                }

                var result = await _authService.RegisterAsync(userRegisterDto);

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isLoginSuccessful = await _authService.LoginAsync(userLoginDto);

            if (!isLoginSuccessful)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            _tokenStorageService.RevokeTokens();
            return NoContent();
        }


        [HttpPost()]
        public async Task<IActionResult> RefreshToken()
        {
            var isIssued = await _tokenService.RefreshTokensAsync(HttpContext);

            if (!isIssued)
            {
                return Unauthorized("The refresh token has expired");
            }

            return Ok();
        }
    }
}

