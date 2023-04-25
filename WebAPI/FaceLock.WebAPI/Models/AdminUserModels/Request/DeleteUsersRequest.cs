using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    public class DeleteUsersRequest
    {
        public IEnumerable<string> UsersId { get; set; }
    }
}
