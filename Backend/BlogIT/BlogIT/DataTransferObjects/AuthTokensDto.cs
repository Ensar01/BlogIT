using BlogIT.Data.Models;

namespace BlogIT.DataTransferObjects
{
    public record AuthTokensDto (string Token, string RefreshToken);
}
