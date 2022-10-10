using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
