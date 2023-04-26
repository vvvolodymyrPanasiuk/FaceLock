using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    public class DeleteUsersRequest
    {
        [Required(ErrorMessage = "Users Id is required")]
        public IEnumerable<string> UsersId { get; set; }
    }
}
