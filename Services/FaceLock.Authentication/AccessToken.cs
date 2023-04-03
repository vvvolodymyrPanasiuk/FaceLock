namespace FaceLock.Authentication
{
    public class AccessToken
    {
        public string? Token { get; set; }
        public DateTime AccessTokenExpires { get; set; }
    }
}
