using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    /// <summary>
    /// Request object for updating user access to a door lock.
    /// </summary>
    public class UpdateAccessDoorLockRequest
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        /// <remarks>
        /// The ID of the user for whom the access to the door lock needs to be updated.
        /// </remarks>
        /// <example>
        /// "1234-5678-9012-3456"
        /// </example>
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        /// <summary>
        /// The ID of the door lock.
        /// </summary>
        /// <remarks>
        /// The ID of the door lock for which the access needs to be updated.
        /// </remarks>
        /// <example>
        /// 123
        /// </example>
        [Required(ErrorMessage = "Door lock ID is required")]
        public int DoorLockId { get; set; }

        /// <summary>
        /// The updated access status for the user and door lock.
        /// </summary>
        /// <remarks>
        /// Whether the user should be granted access to the door lock or not.
        /// </remarks>
        /// <example>
        /// true
        /// </example>
        [Required(ErrorMessage = "Set access to door lock is required")]
        public bool HasAccess { get; set; }
    }
}
