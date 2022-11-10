using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.CustomValidation
{
    public class SecurityCodeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string codeString = value.ToString();
            if(codeString.Length != 3)
            {
                return new ValidationResult("The Card Security Code is invalid");
            }
            return ValidationResult.Success;
        }
    }
}
