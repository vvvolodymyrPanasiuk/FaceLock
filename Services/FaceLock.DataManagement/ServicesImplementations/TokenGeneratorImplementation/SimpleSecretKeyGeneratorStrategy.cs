using FaceLock.DataManagement.Services;

namespace FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation
{
    public class SimpleSecretKeyGeneratorStrategy : ISecretKeyGeneratorService
    {
        public string GenerateSecretKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
