using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class ItemData
    {
        public ProductTypeEnum Type { get; set; }

        public string Name { get; set; }
        public HedgeEnum Risk { get; set; }

        public string Sku { get; set; }
        public long UnitPrice { get; set; }
        public int Quantity { get; set; }

        public HedgeEnum HostHedge { get; set; }
        public HedgeEnum NonSensicalHedge { get; set; }
        public HedgeEnum ObscenitiesHedge { get; set; }
        public HedgeEnum PhoneHedge { get; set; }
        public HedgeEnum TimeHedge { get; set; }
        public HedgeEnum VelocityHedge { get; set; }
        public GiftCategoryEnum GiftCategory { get; set; }

        public PassengerData Passenger { get; set; }
    }
}
