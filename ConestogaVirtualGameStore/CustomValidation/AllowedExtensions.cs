using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ConestogaVirtualGameStore.CustomValidation
{
    public class AllowedExtensions : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensions(string[] extensions)
        {
            _extensions = extensions;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var image = value as IFormFile;
            if(image != null)
            {
                var extension = Path.GetExtension(image.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult("This image format is not allowed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
