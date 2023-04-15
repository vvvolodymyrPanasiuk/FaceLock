using FaceLock.Authentication.Repositories;
using FaceLock.Authentication.Services;
using FaceLock.Domain.Entities.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FaceLock.Authentication.ServicesImplementations
{
    /// <summary>
    /// Token service implementation interface ITokenService for generating, refreshing and revoking JWT tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSettings _jwtTokenSetting;
        private readonly IBlacklistRepository _blacklistRepository;
        private readonly ITokenStateRepository _tokenStateRepository;
        private readonly UserManager<User> _userManager;

        public TokenService(
            IOptions<JwtTokenSettings> jwtTokenSetting, 
            IBlacklistRepository blacklistRepository, 
            UserManager<User> userManager,
            ITokenStateRepository tokenStateRepository)
        {
            _jwtTokenSetting = jwtTokenSetting.Value;
            _blacklistRepository = blacklistRepository;
            _tokenStateRepository = tokenStateRepository;
            _userManager = userManager;
        }


        public async Task<string> GenerateAccessToken(User user)
        {
            try
            {
                // Define claims for the JWT token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Role, user.Status)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtTokenSetting.SecretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _jwtTokenSetting.Issuer,
                    Audience = _jwtTokenSetting.Audience,
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtTokenSetting.AccessTokenExpirationMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                return await Task.Run(() => tokenHandler.WriteToken(token));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error generating JWT token, error: {ex.Message}");
            }
        }

        public async Task<string> GenerateRefreshToken(string userId)
        {
            try
            {
                // Generate token
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);

                // Save token in store
                await _tokenStateRepository.AddRefreshTokenAsync(new RefreshToken
                {
                    Token = refreshToken,
                    RefreshTokenExpires = DateTime.UtcNow.AddDays(_jwtTokenSetting.RefreshTokenExpirationDays),
                    UserId = userId
                });

                return await Task.Run(() => refreshToken);  
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Error generating JWT token, error: {ex.Message}");
            }
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            if (await _tokenStateRepository.IsRefreshTokenValidAsync(refreshToken))
            {
                throw new ApplicationException($"Refresh token not found.");
            }

            await _tokenStateRepository.RemoveRefreshTokenAsync(refreshToken);
            return await _blacklistRepository.
                AddTokenToBlacklistAsync(refreshToken, TimeSpan.FromDays(_jwtTokenSetting.RefreshTokenExpirationDays));
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            var refreshTokenData = await _tokenStateRepository.GetRefreshTokenAsync(refreshToken);

            if (refreshTokenData == null)
            {
                throw new ApplicationException("Refresh token not found.");
            }
            if (refreshTokenData.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new ApplicationException("Refresh token has expired.");
            }
            if (await _blacklistRepository.IsTokenInBlacklistAsync(refreshToken))
            {
                throw new ApplicationException("Refresh token has been revoked.");
            }

            var user = await _userManager.FindByIdAsync(refreshTokenData.UserId);
            if (user == null)
            {
                throw new ApplicationException("User not found.");
            }

            var accessToken = await GenerateAccessToken(user);
            return accessToken;
        }

        public async Task<ClaimsPrincipal> GetPrincipalFromAccessToken(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtTokenSetting.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return await Task.Run(() => principal);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error get principal from token, error: {ex.Message}");
            }
        }    
    }
}
