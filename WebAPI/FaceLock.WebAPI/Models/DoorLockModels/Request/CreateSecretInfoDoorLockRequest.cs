using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    /// <summary>
    /// Request object to create a new secret info of door lock.
    /// </summary>
    public class CreateSecretInfoDoorLockRequest
    {
        /// <summary>
        /// Url connection to WebSocket.
        /// </summary>
        /// <remarks>
        /// Url connection to door lock by WebSocket.
        /// </remarks>
        /// <example>
        /// "wss://example.com/my-namespace"
        /// </example>
        [Required(ErrorMessage = "UrlConnection of door lock is required")]
        [DataType(DataType.Url)]
        public string UrlConnection { get; set; }

        /// <summary>
        /// Door lock ID.
        /// </summary>
        /// <remarks>
        /// The ID of the door lock to which access is being granted or denied.
        /// </remarks>
        /// <example>
        /// 1
        /// </example>
        [Required(ErrorMessage = "Door lock ID is required")]
        public int DoorLockId { get; set; }

        /// <summary>
        /// Secret key to generate token.
        /// </summary>
        /// <remarks>
        /// Secret key to generate token for door lock by WebSocket.
        /// </remarks>
        /// <example>
        /// fisboGklKbop32,rokm43qolm/!ol,tbgs
        /// </example>
        public string SecretKey { get; set; }
    }
}
