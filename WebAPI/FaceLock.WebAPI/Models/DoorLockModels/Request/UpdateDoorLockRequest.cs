using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    /// <summary>
    /// Request object to update a door lock.
    /// </summary>
    public class UpdateDoorLockRequest
    {
        /// <summary>
        /// Name of the door lock.
        /// </summary>
        /// <remarks>
        /// The name of the door lock to be updated.
        /// </remarks>
        /// <example>
        /// Door lock 297
        /// </example>
        [Required(ErrorMessage = "Name of door lock is required")]
        [StringLength(50, ErrorMessage = "Name of door lock cannot exceed 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the door lock.
        /// </summary>
        /// <remarks>
        /// The description of the door lock to be updated.
        /// </remarks>
        /// <example>
        /// Main entrance to the building.
        /// </example>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }
    }
}
