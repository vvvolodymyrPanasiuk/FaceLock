namespace FaceLock.WebAPI.Models.AuthenticationModels.Response
{
    /// <summary>
    /// The response object returned when refreshing an access token.
    /// </summary>
    public class RefreshResponse
    {
        /// <summary>
        /// The newly generated access token.
        /// </summary>
        /// <remarks>
        /// The access token is a string of characters that authenticates the user and allows access to protected resources.
        /// </remarks>
        /// <example>
        /// eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
        /// </example>
        public string AccessToken { get; set; }

        public RefreshResponse(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
