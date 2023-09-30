namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    /// <summary>
    /// Response object for getting door lock secret info.
    /// </summary>
    public class GetDoorLockSecretInfoResponse
    {
        /// <summary>
        /// The ID of the door lock secret info.
        /// </summary>
        /// <remarks>
        /// This is a unique identifier for the door lock.
        /// </remarks>
        /// <example>
        /// 1
        /// </example>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the door lock.
        /// </summary>
        /// <remarks>
        /// This is a unique identifier for the door lock.
        /// </remarks>
        /// <example>
        /// 1
        /// </example>
        public int DoorLockId { get; set; }

        /// <summary>
        /// Url connection to WebSocket.
        /// </summary>
        /// <remarks>
        /// Url connection to door lock by WebSocket.
        /// </remarks>
        /// <example>
        /// "wss://example.com/my-namespace"
        /// </example>
        public string SerialNumber { get; set; }


        public GetDoorLockSecretInfoResponse(int id, int doorLock, string serialNumber)
        {
            Id = id;
            DoorLockId = doorLock;
            SerialNumber = serialNumber;
        }
    }
}
