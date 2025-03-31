using Azure;
using BlogIT.DataTransferObjects;
using BlogIT.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public void RevokeTokens()
        {
            var context = _httpContextAccessor.HttpContext;

            context.Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            context.Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }
    }
}
