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
        /// <param name="serialNumber">The URL/connection information of the lock.</param>
        /// <returns>True if the message was successfully sent; otherwise, false.</returns>
        Task<bool> SendToLockAsync(string serialNumber);

        Task AddToWhiteListAsync(string serialNumber);
    }
}