namespace ConestogaVirtualGameStore.Models
{
    public class ShippingAddress
    {
        public int ShippingAddressId { get; set; }
        public int AddressModelId { get; set; }
        public string ShipAddress1 { get; set; }
        public string ShipAddress2 { get; set; }
        public int? ShipCountry { get; set; }
        public int? ShipProvince { get; set; }
        public int? ShipCity { get; set; }

        public virtual AddressModel AddressModel { get; set; }
        public virtual Country Country { get; set; }
        public virtual Province Province { get; set; }
        public virtual City City { get; set; }
    }
}
