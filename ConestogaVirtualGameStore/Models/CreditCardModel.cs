using System;
using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class CreditCardModel
    {
        public int CreditCardModelId { get; set; }
        public long CreditCardNumber { get; set; }
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
        public int SecurityCode { get; set; }


        public virtual ApplicationUser User { get; set; }
    }
}
