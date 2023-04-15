using FaceLock.Authentication.DTO;
using FaceLock.Domain.Entities.UserAggregate;
using System.Security.Claims;

namespace FaceLock.Authentication.Services
{
    /// <summary>
    /// Interface for token service which is responsible for generating, refreshing, and validating access tokens and refresh tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates an access token for the provided user.
        /// </summary>
        /// <param name="user">User for whom the access token needs to be generated.</param>
        /// <returns>Generated access token as a string.</returns>
        Task<string> GenerateAccessToken(User user);
        /// <summary>
        /// Generates a refresh token for the provided user.
        /// </summary>
        /// <param name="userId">User ID for whom the refresh token needs to be generated.</param>
        /// <param name="userMetaDataDto">User metadata for additional information associated with the refresh token.</param>
        /// <returns>Generated refresh token as a string.</returns>
        Task<string> GenerateRefreshToken(string userId, UserMetaDataDTO userMetaDataDto);
        /// <summary>
        /// Refreshes an access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token used to refresh the access token.</param>
        /// <returns>Refreshed access token as a string.</returns>
        Task<string> RefreshAccessToken(string refreshToken);
        /// <summary>
        /// Retrieves the principal claims from the provided access token.
        /// </summary>
        /// <param name="accessToken">Access token from which to retrieve the claims.</param>
        /// <returns>Claims principal representing the claims from the access token.</returns>
        Task<ClaimsPrincipal> GetPrincipalFromAccessToken(string accessToken);
        /// <summary>
        /// Revokes a refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token to be revoked.</param>
        /// <returns>True if the refresh token was successfully revoked, false otherwise.</returns>
        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
