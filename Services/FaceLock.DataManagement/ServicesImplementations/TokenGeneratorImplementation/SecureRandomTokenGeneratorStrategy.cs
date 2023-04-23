using FaceLock.DataManagement.Services;
using System.Security.Cryptography;

namespace FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation
{
    public class SecureRandomTokenGeneratorStrategy : ITokenGeneratorService
    {
        private const int TokenLength = 32; // length token in byte

        public async Task<string> GenerateTokenAsync()
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var tokenBytes = new byte[TokenLength];
            randomNumberGenerator.GetBytes(tokenBytes);
            return await Task.FromResult(Convert.ToBase64String(tokenBytes));
        }
    }
}
