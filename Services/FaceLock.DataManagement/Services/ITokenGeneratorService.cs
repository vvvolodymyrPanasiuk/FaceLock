using FaceLock.Domain.Entities.DoorLockAggregate;

namespace FaceLock.DataManagement.Services
{
    /// <summary>
    /// Interface for a token generator service that provides methods to generate JWT tokens.
    /// </summary>
    public interface ITokenGeneratorService
    {
        /// <summary>
        /// Generates a JWT token with the given secret key.
        /// </summary>
        /// <param name="secretKey">The secret key used for token generation.</param>
        /// <returns>The generated JWT token.</returns>
        string GenerateToken(string secretKey);

        /// <summary>
        /// Generates a JWT token with the given secret key and door lock information.
        /// </summary>
        /// <param name="secretKey">The secret key used for token generation.</param>
        /// <param name="doorLockId">The door lock identifier.</param>
        /// <returns>The generated JWT token.</returns>
        string GenerateToken(string secretKey, int doorLockId);

        /// <summary>
        /// Generates a JWT token with the given secret key, door lock information, and URL connection.
        /// </summary>
        /// <param name="secretKey">The secret key used for token generation.</param>
        /// <param name="doorLockId">The door lock identifier.</param>
        /// <param name="urlConnection">The URL connection information.</param>
        /// <returns>The generated JWT token.</returns>
        string GenerateToken(string secretKey, int doorLockId, string urlConnection);

        /// <summary>
        /// Generates a JWT token using the provided DoorLockSecurityInfo object.
        /// </summary>
        /// <param name="doorLockSecurityInfo">The security information related to the door lock.</param>
        /// <returns>The generated JWT token.</returns>
        string GenerateToken(DoorLockSecurityInfo doorLockSecurityInfo);
    }
}
