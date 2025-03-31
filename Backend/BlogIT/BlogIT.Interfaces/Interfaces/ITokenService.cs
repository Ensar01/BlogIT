using BlogIT.Model.DataTransferObjects;

namespace BlogIT.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserTokenDto user);
        string GenerateRefreshToken();
    }
}
