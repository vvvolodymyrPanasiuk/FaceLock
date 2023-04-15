namespace FaceLock.Authentication
{
    /// <summary>
    /// Represents the JWT token settings used for authentication.
    /// </summary>
    public class JwtTokenSettings
    {
        /// <summary>
        /// Gets or sets the secret key used to generate JWT tokens.
        /// </summary>
        public string? SecretKey { get; set; }
        /// <summary>
        /// Gets or sets the issuer of the JWT tokens.
        /// </summary>
        public string? Issuer { get; set; }
        /// <summary>
        /// Gets or sets the audience of the JWT tokens.
        /// </summary>
        public string? Audience { get; set; }
        /// <summary>
        /// Gets or sets the expiration time in minutes for access tokens.
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }
        /// <summary>
        /// Gets or sets the expiration time in days for refresh tokens.
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; }
    }
}
