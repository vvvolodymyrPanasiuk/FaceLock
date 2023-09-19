using System.Threading.Tasks;

namespace FaceLock.WebSocket.LockCommunicationService
{
    /// <summary>
    /// Interface for communication with smart locks.
    /// </summary>
    public interface ILockCommunicationStrategy
    {
        /// <summary>
        /// Send a message to a lock.
        /// </summary>
        /// <param name="url">The URL/connection information of the lock.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="jwtToken">The JWT token for authentication.</param>
        /// <returns>True if the message was successfully sent; otherwise, false.</returns>
        Task<bool> SendToLockAsync(string url, string message, string jwtToken);
    }
}