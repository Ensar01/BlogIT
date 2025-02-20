using BlogIT.Data.Models;

namespace BlogIT.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
