namespace FaceLock.Authentication.Repositories
{
    public interface ITokenStateRepository
    {
        Task<List<RefreshToken>> GetRefreshTokenByUserIdAsync(string userId);
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        Task RemoveRefreshTokenAsync(string refreshToken);
    }
}
