using ConestogaVirtualGameStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConestogaVirtualGameStore.CustomValidation
{
    public class UniqueCreditNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _context = (GameStoreContext)validationContext.GetService(typeof(GameStoreContext));
            long cardNumber = (long)value;
            var card = _context.CreditCards.Where(a => a.CreditCardNumber == cardNumber).FirstOrDefault();
            if (card != null)
            {
                return new ValidationResult("The credit card number is already in the database. If you didn't register this card send us a report and call your bank");
            }
            return ValidationResult.Success;
        }
    }
}
