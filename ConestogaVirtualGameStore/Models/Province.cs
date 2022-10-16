using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models
{
    public class Province
    {
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<MailingAddress> MailingAddresses { get; set; }
        public ICollection<ShippingAddress> ShippingAddresses { get; set; }
    }
}
