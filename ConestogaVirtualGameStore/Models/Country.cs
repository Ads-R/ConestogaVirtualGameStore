using System.Collections.Generic;

namespace ConestogaVirtualGameStore.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public ICollection<Province> Provinces { get; set; }
        public ICollection<MailingAddress> MailingAddresses { get; set; }
        public ICollection<ShippingAddress> ShippingAddresses { get; set; }
    }
}
