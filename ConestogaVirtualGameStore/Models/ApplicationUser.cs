using Microsoft.AspNetCore.Identity;

namespace ConestogaVirtualGameStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ProfileModel Profile { get; set; }
        public virtual PreferencesModel Preference { get; set; }
        public virtual AddressModel Address { get; set; }
    }
}
