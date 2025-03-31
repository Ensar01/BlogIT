using BlogIT.Data.Models;
using BlogIT.Data;
using BlogIT.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogIT.DataTransferObjects;
using BlogIT.Model.DataTransferObjects;

namespace BlogIT.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<RefreshToken> CreateOrUpdateRefreshToken(UserTokenDto user)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.UserId == user.Id);

            if (refreshToken != null)
            {
                refreshToken.Token = _tokenService.GenerateRefreshToken();
                refreshToken.ExpiresOn = DateTime.UtcNow.AddDays(7);
            }
            else
            {
                refreshToken = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Token = _tokenService.GenerateRefreshToken(),
                    ExpiresOn = DateTime.UtcNow.AddDays(7)
                };
                await _context.RefreshTokens.AddAsync(refreshToken);
            }

            await _context.SaveChangesAsync();
            return refreshToken;
        }
        
        public async Task<AuthTokensDto> GenerateTokens(UserTokenDto user)
        {
            string token = _tokenService.GenerateToken(user);
            var refreshToken = await CreateOrUpdateRefreshToken(user);

            return new AuthTokensDto(token, refreshToken.Token);
        }
    }
}

