using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.PlaceModels.Request
{
    public class UpdatePlaceRequest
    {
        [Required(ErrorMessage = "Name of place is required")]
        [StringLength(50, ErrorMessage = "Name of place cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }
    }
}
