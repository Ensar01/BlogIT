using BlogIT.DataTransferObjects;
using BlogIT.Model.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace BlogIT.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserTokenDto user);
        string GenerateRefreshToken();
        Task<AuthTokensDto> GenerateTokens(UserTokenDto user);
        Task<AuthTokensDto?> RefreshTokensAsync(HttpContext httpContext);
    }
}
