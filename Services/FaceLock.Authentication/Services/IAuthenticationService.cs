using FaceLock.Authentication.DTO;

namespace FaceLock.Authentication.Services
{
    /// <summary>
    /// Interface for authentication service that provides methods for user authentication and authorization.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates a user with the provided login credentials and returns access token and refresh token.
        /// </summary>
        /// <param name="userLoginDto">User login data transfer object.</param>
        /// <returns>A tuple containing the access token and refresh token.</returns>
        Task<(string, string)> LoginAsync(UserLoginDTO userLoginDto);
        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="userRegisterDto">User registration data transfer object.</param>
        /// <returns>True if the user is successfully registered, false otherwise.</returns>
        Task<bool> RegisterAsync(UserRegisterDTO userRegisterDto);
        /// <summary>
        /// Logs out a user by invalidating the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token to be invalidated.</param>
        /// <returns>True if the refresh token is successfully invalidated, false otherwise.</returns>
        Task<bool> LogoutAsync(string refreshToken);
        /// <summary>
        /// Refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">Refresh token to be used for token refreshing.</param>
        /// <returns>The new access token.</returns>
        Task<string> RefreshAccessTokenAsync(string refreshToken);
    }
}
