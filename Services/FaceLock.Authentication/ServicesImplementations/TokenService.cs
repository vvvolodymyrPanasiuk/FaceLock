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
    public class TokenService : ITokenService
    {
        private readonly JwtTokenSettings _jwtTokenSetting;
        private readonly IBlacklistRepository _blacklistRepository;
        private readonly UserManager<User> _userManager;

        public TokenService(
            IOptions<JwtTokenSettings> jwtTokenSetting, 
            IBlacklistRepository blacklistRepository, 
            UserManager<User> userManager)
        {
            _jwtTokenSetting = jwtTokenSetting.Value;
            _blacklistRepository = blacklistRepository;
            _userManager = userManager;
        }


        public async Task<AccessToken> GenerateAccessToken(User user)
        {
            try
            {
                // Define claims for the JWT token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Role, user.Status)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                // Use a symmetric key to sign the JWT token (UTF8 ??)
                var key = Encoding.ASCII.GetBytes(_jwtTokenSetting.SecretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _jwtTokenSetting.Issuer,
                    Audience = _jwtTokenSetting.Audience,
                    //Claims = (IDictionary<string, object>)claims,              
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtTokenSetting.AccessTokenExpirationMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                return new AccessToken
                {
                    Token = await Task.Run(() => tokenHandler.WriteToken(token)),
                    AccessTokenExpires = DateTime.UtcNow.AddMinutes(_jwtTokenSetting.AccessTokenExpirationMinutes).ToUniversalTime()
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error generating JWT token, error: {ex.Message}");
            }
        }

        public async Task<RefreshToken> GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);
                return await Task.Run(() => new RefreshToken
                {
                    Token = refreshToken,
                    RefreshTokenExpires = DateTime.UtcNow.AddMinutes(_jwtTokenSetting.RefreshTokenExpirationDays).ToUniversalTime(),
                    RefreshTokenIsExpired = false
                });  
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Error generating JWT token, error: {ex.Message}");
            }
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            return await _blacklistRepository.
                AddTokenToBlacklistAsync(refreshToken, TimeSpan.FromDays(_jwtTokenSetting.RefreshTokenExpirationDays));
        }

        public async Task<AccessToken> RefreshAccessToken(RefreshToken refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenSetting.SecretKey);

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(refreshToken.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            }, out validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub).Value;

            if (await _blacklistRepository.IsTokenInBlacklistAsync(refreshToken.Token) == true)
            {
                throw new ApplicationException("Refresh token has been revoked.");
            }

            if (refreshToken.RefreshTokenExpires < DateTime.UtcNow)
            {
                throw new ApplicationException("Refresh token has expired.");
            }

            var user = await _userManager.FindByIdAsync(userId);

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

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
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
