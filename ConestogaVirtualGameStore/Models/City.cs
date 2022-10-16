using System.Collections;
using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        public ICollection<MailingAddress> MailingAddresses { get; set; }
        public ICollection<ShippingAddress> ShippingAddresses { get; set; }
    }
}
