using FaceLock.DataManagement.Services;
using FaceLock.Domain.Entities.DoorLockAggregate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation
{
    public class JwtTokenGeneratorService : ITokenGeneratorService
    {
        public string GenerateToken(string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string secretKey, int doorLock)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, doorLock.ToString()),
                new Claim(ClaimTypes.UserData, DateTime.UtcNow.ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string secretKey, int doorLock, string urlConnection)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, doorLock.ToString()),
                new Claim(ClaimTypes.UserData, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Uri, urlConnection)
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(DoorLockSecurityInfo doorLockSecurityInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(doorLockSecurityInfo.SecretKey);

            var claims = new ClaimsIdentity(new[]
{
                new Claim(ClaimTypes.NameIdentifier, doorLockSecurityInfo.DoorLockId.ToString()),
                new Claim(ClaimTypes.UserData, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Uri, doorLockSecurityInfo.UrlConnection)
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
