using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models
{
    public class ShippingAddress
    {
        public int ShippingAddressId { get; set; }
        public int AddressModelId { get; set; }
        [Display(Name = "Shipping Address Line 1")]
        public string ShipAddress1 { get; set; }
        [Display(Name = "Shipping Address Line 2")]
        public string ShipAddress2 { get; set; }
        [Display(Name = "Ship Country")]
        public int? ShipCountry { get; set; }
        [Display(Name = "Ship Province")]
        public int? ShipProvince { get; set; }
        [Display(Name = "Ship City")]
        public int? ShipCity { get; set; }

        public virtual AddressModel AddressModel { get; set; }
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
    }
}
