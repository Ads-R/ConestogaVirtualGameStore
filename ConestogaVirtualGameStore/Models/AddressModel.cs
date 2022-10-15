using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models
{
    public class AddressModel
    {
        public int AddressModelId { get; set; }
        public string UserId { get; set; }
        [Display(Name ="Mailing Address")]
        public string MailingAddress { get; set; }
        [Display(Name ="Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name ="Is same?")]
        public bool IsSame { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
