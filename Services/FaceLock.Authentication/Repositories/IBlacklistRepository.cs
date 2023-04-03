namespace FaceLock.Authentication.Repositories
{
    public interface IBlacklistRepository
    {
        Task<bool> IsTokenInBlacklistAsync(string refreshToken);
        Task<bool> AddTokenToBlacklistAsync(string refreshToken, TimeSpan expirationTime);
    }
}
