using BlogIT.DataTransferObjects;

namespace BlogIT.Interfaces
{
    public interface ITokenStorageService
    {
        void SetTokens(AuthTokensDto tokensDto);
        void RevokeTokens();
    }
}
