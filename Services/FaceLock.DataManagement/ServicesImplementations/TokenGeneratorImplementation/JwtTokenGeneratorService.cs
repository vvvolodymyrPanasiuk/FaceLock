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
            if(secretKey == null)
            {
                throw new Exception("Door lock security information not exist");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            if(key == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: key cannot be null");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            if(tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token descriptor cannot be null");
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token cannot be null");
            }

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string secretKey, int doorLockId)
        {
            if (secretKey == null)
            {
                throw new Exception("Door lock security information not exist");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            if (key == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: key cannot be null");
            }

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, doorLockId.ToString()),
                new Claim(ClaimTypes.UserData, DateTime.UtcNow.ToString())
            });
            if (claims == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: claims cannot be null");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token descriptor cannot be null");
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token cannot be null");
            }

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(string secretKey, int doorLockId, string urlConnection)
        {
            if (secretKey == null || doorLockId == null || urlConnection == null)
            {
                throw new Exception("Door lock security information not exist");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            if (key == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: key cannot be null");
            }

            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, doorLockId.ToString()),
                new Claim(ClaimTypes.UserData, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Uri, urlConnection)
            });
            if (claims == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: claims cannot be null");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token descriptor cannot be null");
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token cannot be null");
            }

            return tokenHandler.WriteToken(token);
        }

        public string GenerateToken(DoorLockSecurityInfo doorLockSecurityInfo)
        {
            if (doorLockSecurityInfo.SecretKey == null || doorLockSecurityInfo?.DoorLockId == null || doorLockSecurityInfo.UrlConnection == null)
            {
                throw new Exception("Door lock security information not exist");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(doorLockSecurityInfo.SecretKey);
            if (key == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: key cannot be null");
            }

            var claims = new ClaimsIdentity(new[]
{
                new Claim(ClaimTypes.NameIdentifier, doorLockSecurityInfo.DoorLockId.ToString()),
                new Claim(ClaimTypes.UserData, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Uri, doorLockSecurityInfo.UrlConnection)
            });
            if (claims == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: claims cannot be null");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token descriptor cannot be null");
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            if (tokenDescriptor == null)
            {
                throw new Exception("JwtTokenGeneratorService exception: token cannot be null");
            }

            return tokenHandler.WriteToken(token);
        }
    }
}
