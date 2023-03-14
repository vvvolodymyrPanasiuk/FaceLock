using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FaceLock.WebAPI.Validators
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as IFormFileCollection;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileExtension = System.IO.Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(fileExtension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Only {string.Join(", ", _extensions)} files are allowed.";
        }
    }
}
