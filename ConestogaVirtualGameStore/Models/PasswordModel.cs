using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class PasswordModel
    {
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("NewPassword", ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
