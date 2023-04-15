namespace FaceLock.Authentication
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime RefreshTokenExpires { get; set; }
    }
}
