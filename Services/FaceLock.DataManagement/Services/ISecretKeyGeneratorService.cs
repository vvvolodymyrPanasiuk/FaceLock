namespace FaceLock.DataManagement.Services
{
    /// <summary>
    /// Represents a service for generating secret key.
    /// </summary>
    public interface ISecretKeyGeneratorService
    {
        /// <summary>
        /// Generates a new secret key.
        /// </summary>
        string GenerateSecretKey();
    }
}
