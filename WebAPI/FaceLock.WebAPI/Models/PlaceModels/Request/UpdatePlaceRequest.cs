using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.PlaceModels.Request
{
    /// <summary>
    /// Request object used for updating a place.
    /// </summary>
    public class UpdatePlaceRequest
    {
        /// <summary>
        /// Name of the place to be updated.
        /// </summary>
        /// <remarks>
        /// The name of the place that will be updated.
        /// </remarks>
        /// <example>
        /// New York City Streat
        /// </example>
        [Required(ErrorMessage = "Name of place is required")]
        [StringLength(50, ErrorMessage = "Name of place cannot exceed 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the place to be updated.
        /// </summary>s
        /// <remarks>
        /// The description of the place that will be updated.
        /// </remarks>
        /// <example>
        /// The Streat that never sleeps.
        /// </example>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }
    }
}
