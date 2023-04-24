using FaceLock.WebAPI.Validators;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.AdminUserModels.Request
{
    public class AddUserPhotosRequest
    {
        [Required(ErrorMessage = "Please upload at least one photo")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 5MB")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only JPG, JPEG, and PNG files are allowed")]
        public IFormFileCollection Files { get; set; }
    }
}
