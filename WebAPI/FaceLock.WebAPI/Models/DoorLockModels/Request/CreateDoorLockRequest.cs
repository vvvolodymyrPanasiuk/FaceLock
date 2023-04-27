using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    public class CreateDoorLockRequest
    {
        [Required(ErrorMessage = "Name of door lock is required")]
        [StringLength(50, ErrorMessage = "Name of door lock cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }
    }
}
