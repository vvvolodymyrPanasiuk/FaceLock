using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    /// <summary>
    /// Represents a request to delete a user's photos.
    /// </summary>
    public class DeleteUserPhotosRequest
    {
        /// <summary>
        /// Gets or sets the IDs of the user faces to be deleted.
        /// </summary>
        /// <remarks>
        /// The IDs of the user faces to delete.
        /// </remarks>
        [Required(ErrorMessage = "User faces Id is required")]
        public IEnumerable<int> userFacesId { get; set; }
    }
}
