namespace FaceLock.Authentication
{
    public class RefreshToken
    {
        public string? Token { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
        public bool RefreshTokenIsExpired { get; set; }
    }
}
