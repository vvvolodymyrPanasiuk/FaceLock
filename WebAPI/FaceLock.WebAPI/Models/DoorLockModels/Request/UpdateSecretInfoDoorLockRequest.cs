using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    /// <summary>
    /// Request object for updating secret info of door lock.
    /// </summary>
    public class UpdateSecretInfoDoorLockRequest
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
        [Required(ErrorMessage = "Door lock secret info ID is required")]
        public int Id { get; set; }

        /// <summary>
        /// The secret key of the door lock.
        /// </summary>
        /// <remarks>
        /// This is the secret key for generate jwt tokens.
        /// </remarks>
        /// <example>
        /// srtgbjmti;srmbjrts457yt7n748tysrn4sr98tn
        /// </example>
        [Required(ErrorMessage = "Secret key of door lock is required")]
        public string SecretKey { get; set; }

        /// <summary>
        /// Url connection to WebSocket.
        /// </summary>
        /// <remarks>
        /// Url connection to door lock by WebSocket.
        /// </remarks>
        /// <example>
        /// "wss://example.com/my-namespace"
        /// </example>
        [Required(ErrorMessage = "Url Connection of door lock is required")]
        [DataType(DataType.Url)]
        public string UrlConnection { get; set; }

    }
}
