using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class ShippingData
    {
        public string Addressee { get; set; }
        public string Phone { get; set; }
        public ShippingMethodEnum Method { get; set; }
    }
}
