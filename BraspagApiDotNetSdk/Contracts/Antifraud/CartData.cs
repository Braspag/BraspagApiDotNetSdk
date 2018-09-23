using System.Collections.Generic;

namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class CartData
    {
        public bool IsGift { get; set; }
        public bool ReturnsAccepted { get; set; }
        public List<ItemData> Items { get; set; }
    }
}
