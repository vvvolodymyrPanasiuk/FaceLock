using FaceLock.WebAPI.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    /// <summary>
    /// Represents the request object for adding user photo.
    /// </summary>
    public class AddUserPhotoRequest
    {
        /// <summary>
        /// The photo file to upload.
        /// </summary>
        /// <remarks>
        /// Please upload at least one photo.
        /// Maximum allowed file size is 5MB.
        /// Only JPG, JPEG, and PNG files are allowed.
        /// </remarks>
        [Required(ErrorMessage = "Please upload at least one photo")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 5MB")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only JPG, JPEG, and PNG files are allowed")]
        public IFormFile File { get; set; }
    }
}
