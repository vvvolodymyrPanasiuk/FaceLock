using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    /// <summary>
    /// Request object for creating access to a door lock.
    /// </summary>
    public class CreateAccessDoorLockRequest
    {
        /// <summary>
        /// User ID.
        /// </summary>
        /// <remarks>
        /// The ID of the user who is being granted or denied access to the door lock.
        /// </remarks>
        /// <example>
        /// "123456"
        /// </example>
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

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
        /// Access to door lock.
        /// </summary>
        /// <remarks>
        /// Determines whether access is being granted or denied to the door lock.
        /// </remarks>
        /// <example>
        /// true
        /// </example>
        [Required(ErrorMessage = "Set access to door lock is required")]
        public bool HasAccess { get; set; }
    }
}
