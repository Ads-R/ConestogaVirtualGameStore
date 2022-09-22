using Microsoft.AspNetCore.Identity;

namespace ConestogaVirtualGameStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ProfileModel Profile { get; set; }
    }
}
