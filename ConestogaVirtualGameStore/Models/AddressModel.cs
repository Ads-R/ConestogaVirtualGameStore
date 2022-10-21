using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models
{
    public class AddressModel
    {
        public int AddressModelId { get; set; }
        public string UserId { get; set; }
        [Display(Name ="I have the same Mailing and Shipping Address")]
        public bool IsSame { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual MailingAddress MailingAddress { get; set; }
        public virtual ShippingAddress ShippingAddress { get; set; }

    }
}
