using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    /// <summary>
    /// Request object to create a new secret info of door lock.
    /// </summary>
    public class CreateSecretInfoDoorLockRequest
    {
        /// <summary>
        /// SerialNumber of door lock to WebSocket.
        /// </summary>
        /// <remarks>
        /// SerialNumber of door lock by WebSocket.
        /// </remarks>
        /// <example>
        /// "12iuhl-541-uig"
        /// </example>
        [Required(ErrorMessage = "SerialNumber of door lock is required")]
        [DataType(DataType.Text)]
        public string SerialNumber { get; set; }

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
    }
}
