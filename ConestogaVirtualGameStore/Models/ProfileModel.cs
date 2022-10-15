using ConestogaVirtualGameStore.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models
{
    public class ProfileModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        [Display(Name ="Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name ="Is Subscribed?")]
        public bool IsSubscribed { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
