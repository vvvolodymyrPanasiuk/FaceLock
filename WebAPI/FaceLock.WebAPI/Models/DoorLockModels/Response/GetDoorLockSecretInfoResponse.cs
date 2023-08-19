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
        public string UrlConnection { get; set; }

        /// <summary>
        /// The description of the door lock.
        /// </summary>
        /// <remarks>
        /// This is the secret key for generate jwt tokens.
        /// </remarks>
        /// <example>
        /// srtgbjmti;srmbjrts457yt7n748tysrn4sr98tn
        /// </example>
        public string SecretKey { get; set; }

        public GetDoorLockSecretInfoResponse(int id, int doorLock, string urlConnection, string secretKey)
        {
            Id = id;
            DoorLockId = doorLock;
            UrlConnection = urlConnection;
            SecretKey = secretKey;
        }
    }
}
