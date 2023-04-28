using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    /// <summary>
    /// Represents a request to delete multiple users.
    /// </summary>
    public class DeleteUsersRequest
    {
        /// <summary>
        /// Gets or sets the IDs of the users to delete.
        /// </summary>
        /// <remarks>
        /// The IDs of the users to delete.
        /// </remarks>
        [Required(ErrorMessage = "Users Id is required")]
        public IEnumerable<string> UsersId { get; set; }
    }
}
