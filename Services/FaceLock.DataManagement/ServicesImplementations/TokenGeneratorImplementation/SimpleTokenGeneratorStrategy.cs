using FaceLock.DataManagement.Services;

namespace FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation
{
    public class SimpleTokenGeneratorStrategy : ITokenGeneratorService
    {
        public async Task<string> GenerateTokenAsync()
        {
            return await Task.FromResult(Guid.NewGuid().ToString());
        }
    }
}
