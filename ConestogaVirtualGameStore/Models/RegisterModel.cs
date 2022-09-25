using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password", ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
