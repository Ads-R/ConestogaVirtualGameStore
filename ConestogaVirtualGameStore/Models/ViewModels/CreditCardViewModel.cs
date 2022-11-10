using System.ComponentModel.DataAnnotations;
using System;
using ConestogaVirtualGameStore.CustomValidation;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class CreditCardViewModel
    {
        [LuhnValidation]
        [UniqueCreditNumber]
        public long CreditCardNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string ExpiryMonth { get; set; }
        [Required]
        public string ExpiryYear { get; set; }
        [SecurityCodeValidation]
        public int SecurityCode { get; set; }
    }
}
