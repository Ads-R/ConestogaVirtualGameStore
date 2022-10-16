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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dddd, MMMM d, yyyy}")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name ="Receive promotional emails from CVGS")]
        public bool IsSubscribed { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
