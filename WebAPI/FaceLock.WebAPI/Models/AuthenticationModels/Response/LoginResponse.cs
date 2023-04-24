namespace FaceLock.WebAPI.Models.AuthenticationModels.Response
{
    public class LoginResponse
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

        public LoginResponse(string refreshToken, string accessToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
        }
    }
}
