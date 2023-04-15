namespace FaceLock.Authentication.Repositories
{
    /// <summary>
    /// Interface for working with blacklist repository that stores revoked refresh tokens.
    /// </summary>
    public interface IBlacklistRepository
    {
        /// <summary>
        /// Checks if a token is in the blacklist.
        /// </summary>
        /// <param name="refreshToken">The refresh token to check.</param>
        /// <returns>True if the token is in the blacklist, otherwise false.</returns>
        Task<bool> IsTokenInBlacklistAsync(string refreshToken);
        /// <summary>
        /// Adds a token to the blacklist with the specified expiration time.
        /// </summary>
        /// <param name="refreshToken">The refresh token to add to the blacklist.</param>
        /// <param name="expirationTime">The expiration time for the token in the blacklist.</param>
        /// <returns>True if the token was successfully added to the blacklist, otherwise false.</returns>
        Task<bool> AddTokenToBlacklistAsync(string refreshToken, TimeSpan expirationTime);
    }
}
