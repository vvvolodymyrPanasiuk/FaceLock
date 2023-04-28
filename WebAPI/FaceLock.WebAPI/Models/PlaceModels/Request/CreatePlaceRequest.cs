using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.PlaceModels.Request
{
    /// <summary>
    /// Request object for creating a new place.
    /// </summary>
    public class CreatePlaceRequest
    {
        /// <summary>
        /// Name of the place.
        /// </summary>
        /// <remarks>
        /// The name of the place that will be created.
        /// </remarks>
        /// <example>
        /// Central Park
        /// </example>
        [Required(ErrorMessage = "Name of place is required")]
        [StringLength(50, ErrorMessage = "Name of place cannot exceed 50 characters")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the place.
        /// </summary>
        /// <remarks>
        /// A brief description of the place that will be created.
        /// </remarks>
        /// <example>
        /// Central Park is an urban park in New York City.
        /// </example>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; }
    }
}
