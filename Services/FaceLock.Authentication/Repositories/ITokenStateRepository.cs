namespace FaceLock.Authentication.Repositories
{
    /// <summary>
    /// Interface for token state repository, which is responsible for managing refresh tokens in the system.
    /// </summary>
    public interface ITokenStateRepository
    {
        /// <summary>
        /// Get refresh tokens by user ID asynchronously.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of refresh tokens</returns>
        Task<List<RefreshToken>> GetRefreshTokenByUserIdAsync(string userId);
        /// <summary>
        /// Get refresh token by its value asynchronously.
        /// </summary>
        /// <param name="refreshToken">Refresh token value</param>
        /// <returns>Refresh token</returns>
        Task<RefreshToken> GetRefreshTokenAsync(string refreshToken);
        /// <summary>
        /// Add refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        /// <summary>
        /// Check if a refresh token is valid asynchronously.
        /// </summary>
        /// <param name="refreshToken">Refresh token value</param>
        /// <returns>True if the refresh token is valid, otherwise false</returns>
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        /// <summary>
        /// Remove refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">Refresh token value</param>
        Task RemoveRefreshTokenAsync(string refreshToken);
    }
}
