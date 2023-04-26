using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    public class DeleteUserPhotosRequest
    {
        [Required(ErrorMessage = "User faces Id is required")]
        public IEnumerable<int> userFacesId { get; set; }
    }
}
