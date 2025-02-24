using BlogIT.DataTransferObjects;
using BlogIT.Interfaces;

namespace BlogIT.Services
{
    public class CookieTokenStorageService: ITokenStorageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieTokenStorageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetTokens(AuthTokensDto authTokensDto)
        {
            var context = _httpContextAccessor.HttpContext;

            context.Response.Cookies.Append("accessToken", authTokensDto.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddMinutes(10)
            });

            context.Response.Cookies.Append("refreshToken", authTokensDto.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
