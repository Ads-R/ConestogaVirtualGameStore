using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class CartViewModel
    {
        [Required]
        public int CreditCard { get; set; }
    }
}
