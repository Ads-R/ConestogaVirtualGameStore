using System.ComponentModel.DataAnnotations;

namespace ConestogaVirtualGameStore.Models
{
    public class LoginModel
    {
        [Display(Name ="Display Name")]
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

