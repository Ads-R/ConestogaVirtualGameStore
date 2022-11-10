using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models.ViewModels
{
    public class FriendSearchViewModel
    {
        public string UserId { get; set; }
        public List<ApplicationUser>? Users { get; set; }
        public List<string>? AllFriends { get; set; }
        public List<ApplicationUser>? AcceptedFriends { get; set; }
        public List<ApplicationUser>? PendingApproval { get; set; }
        public List<ApplicationUser>? PendingRequest { get; set; }
    }
}
