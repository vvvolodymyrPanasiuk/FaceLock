namespace FaceLock.Authentication
{
    /// <summary>
    /// Model class for storing refresh token information.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Gets or sets the refresh token value.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Gets or sets the user ID associated with the refresh token.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the expiration date of the refresh token.
        /// </summary>
        public DateTime RefreshTokenExpires { get; set; }
    }
}
