using FaceLock.Domain.Entities.UserAggregate;
using System.Security.Claims;

namespace FaceLock.Authentication.Services
{
    public interface ITokenService
    {
        Task<AccessToken> GenerateAccessToken(User user);
        Task<RefreshToken> GenerateRefreshToken();
        Task<AccessToken> RefreshAccessToken(RefreshToken refreshToken);
        Task<ClaimsPrincipal> GetPrincipalFromAccessToken(string accessToken);
        Task<bool> RevokeRefreshToken(string refreshToken);
    }
}
