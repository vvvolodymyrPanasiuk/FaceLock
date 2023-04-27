using FaceLock.DataManagement.Services;

namespace FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation
{
    public class SimpleTokenGeneratorStrategy : ITokenGeneratorService
    {
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
