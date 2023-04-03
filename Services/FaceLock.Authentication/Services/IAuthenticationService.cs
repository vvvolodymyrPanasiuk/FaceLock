using FaceLock.Authentication.DTO;

namespace FaceLock.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<(string, string)> LoginAsync(UserLoginDTO userLoginDto);
        Task<bool> RegisterAsync(UserRegisterDTO userRegisterDto);
        Task<bool> LogoutAsync(string refreshToken);
    }
}
