using BlogIT.Data;
using BlogIT.Data.Models;
using BlogIT.DataTransferObjects;
using BlogIT.Interfaces;
using BlogIT.Model.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogIT.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly ApplicationDbContext _context;
        private readonly ITokenStorageService _tokenStorageService;

        public TokenService(IConfiguration config, ApplicationDbContext context , ITokenStorageService tokenStorageService)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            _context = context;
            _tokenStorageService = tokenStorageService;
        }
        public string GenerateToken(UserTokenDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.ID)
            };
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = credentials,
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task<RefreshToken> CreateOrUpdateRefreshToken(UserTokenDto user)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.UserId == user.ID);

            if (refreshToken != null)
            {
                refreshToken.Token = GenerateRefreshToken();
                refreshToken.ExpiresOn = DateTime.UtcNow.AddDays(7);
            }
            else
            {
                refreshToken = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.ID,
                    Token = GenerateRefreshToken(),
                    ExpiresOn = DateTime.UtcNow.AddDays(7)
                };
                await _context.RefreshTokens.AddAsync(refreshToken);
            }

            await _context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<AuthTokensDto> GenerateTokens(UserTokenDto user)
        {
            string token = GenerateToken(user);
            var refreshToken = await CreateOrUpdateRefreshToken(user);

            return new AuthTokensDto(token, refreshToken.Token);
        }
        public async Task<AuthTokensDto?> RefreshTokensAsync(HttpContext httpContext)
        {
            httpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

            var tokenEntry = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (tokenEntry is null || tokenEntry.ExpiresOn < DateTime.UtcNow)
            {
                return null;
            }

            var user = tokenEntry.User;
            var userTokenDto = new UserTokenDto(user.Email, user.UserName, user.Id);

            _context.RefreshTokens.Remove(tokenEntry);
            await _context.SaveChangesAsync();

            var newTokens = await GenerateTokens(userTokenDto);

            _tokenStorageService.SetTokens(newTokens);

            return newTokens;
        }


    }
}

