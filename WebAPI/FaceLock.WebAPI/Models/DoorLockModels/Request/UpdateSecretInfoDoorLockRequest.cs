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
        /// Serial number of door lock to WebSocket.
        /// </summary>
        /// <remarks>
        /// Serial number to door lock by WebSocket.
        /// </remarks>
        /// <example>
        /// "rngy-45tyd-ytnytdn"
        /// </example>
        [Required(ErrorMessage = "Url Connection of door lock is required")]
        [DataType(DataType.Text)]
        public string SerialNumber { get; set; }
    }
}
