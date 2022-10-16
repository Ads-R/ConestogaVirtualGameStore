namespace ConestogaVirtualGameStore.Models
{
    public class MailingAddress
    {
        public int MailingAddressId { get; set; }
        public int AddressModelId { get; set; }
        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public int? MailCountry { get; set; }
        public int? MailProvince { get; set; }
        public int? MailCity { get; set; }

        public virtual AddressModel AddressModel { get; set; }
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
    }
}
