namespace FaceLock.DataManagement.Services
{
    /// <summary>
    /// Represents a service for generating tokens.
    /// </summary>
    public interface ITokenGeneratorService
    {
        /// <summary>
        /// Generates a new token asynchronously.
        /// </summary>
        Task<string> GenerateTokenAsync();
    }
}
