using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ConestogaVirtualGameStore.Models
{
    public class MailingAddress
    {
        public int MailingAddressId { get; set; }
        public int AddressModelId { get; set; }
        [Display(Name = "Mailing Address Line 1")]
        public string MailAddress1 { get; set; }
        [Display(Name = "Mailing Address Line 2")]
        public string MailAddress2 { get; set; }
        [Display(Name = "Mail Country")]
        public int? MailCountry { get; set; }
        [Display(Name = "Mail Province")]
        public int? MailProvince { get; set; }
        [Display(Name = "Mail City")]
        public int? MailCity { get; set; }

        public virtual AddressModel AddressModel { get; set; }
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
    }
}
