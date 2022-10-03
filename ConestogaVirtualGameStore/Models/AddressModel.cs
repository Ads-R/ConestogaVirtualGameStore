namespace ConestogaVirtualGameStore.Models
{
    public class AddressModel
    {
        public int AddressModelId { get; set; }
        public string UserId { get; set; }
        public string MailingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public bool IsSame { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
