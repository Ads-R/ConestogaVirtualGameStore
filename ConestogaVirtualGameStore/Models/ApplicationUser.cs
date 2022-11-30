using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ProfileModel Profile { get; set; }
        public virtual PreferencesModel Preference { get; set; }
        public virtual AddressModel Address { get; set; }
        public ICollection<ReviewModel> Reviews { get; set; }
        public ICollection<RatingModel> Ratings { get; set; }
        public ICollection<CreditCardModel> CreditCards { get; set; }
        public ICollection<FriendModel> UserFriend { get; set; }
        public ICollection<FriendModel> Friend { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public ICollection<EventParticipantsModel> EventParticipants { get; set; }

        public ICollection<WishListModel> Wish { get; set; }

    }
}
