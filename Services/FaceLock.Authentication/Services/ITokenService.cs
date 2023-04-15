using FaceLock.Domain.Entities.UserAggregate;
using System.Security.Claims;

namespace FaceLock.Authentication.Services
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken(string userId);
        Task<string> RefreshAccessToken(string refreshToken);
        Task<ClaimsPrincipal> GetPrincipalFromAccessToken(string accessToken);
        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
