using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    public class DeleteUserPhotosRequest
    {
        public IEnumerable<int> userFacesId { get; set; }
    }
}
