namespace FaceLock.WebAPI.Models.AuthenticationModels.Response
{
    /// <summary>
    /// Login response object.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// A refresh token that can be used to obtain a new access token.
        /// </summary>
        /// <remarks>
        /// This token can be used to obtain a new access token when the current access token expires or is revoked.
        /// </remarks>
        /// <example>
        /// eyJhbGciOiJIUzI1NiIsInR5cCIfwpMeJf36POk6yJV_adQssw5c
        /// </example>
        public string RefreshToken { get; set; }

        /// <summary>
        /// An access token that can be used to access protected resources.
        /// </summary>
        /// <remarks>
        /// This token is used to access protected resources on behalf of the user who authorized the application.
        /// </remarks>
        /// <example>
        /// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
        /// </example>
        public string AccessToken { get; set; }

        public LoginResponse(string refreshToken, string accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }
    }
}
