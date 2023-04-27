using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.DoorLockModels.Request
{
    public class CreateAccessDoorLockRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Door lock ID is required")]
        public int DoorLockId { get; set; }
        [Required(ErrorMessage = "Set access to door lock is required")]
        public bool HasAccess { get; set; }
    }
}
