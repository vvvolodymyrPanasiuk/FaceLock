using FaceLock.WebSocket.Protos;
using System.Threading.Tasks;

namespace FaceLock.WebAPI.Clients.GrpcClient
{
    /// <summary>
    /// Represents a client for interacting with the DoorLock gRPC service.
    /// </summary>
    public interface IGrpcDoorLockClient
    {
        /// <summary>
        /// Opens a door lock using the provided token.
        /// </summary>
        /// <param name="token">The access token for opening the door lock.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response from the gRPC service.</returns>
        Task<DoorLockServiceResponse> OpenDoorLockAsync(string token);
    }
}
