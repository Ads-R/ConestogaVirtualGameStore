using ConestogaVirtualGameStore.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;

namespace ConestogaVirtualGameStore.Models
{
    public class ProfileModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsSubscribed { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
