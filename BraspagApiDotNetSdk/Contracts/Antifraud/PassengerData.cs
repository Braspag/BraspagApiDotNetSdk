using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class PassengerData
    {
        public string Name { get; set; }

        public string Identity { get; set; }
        public string Status { get; set; }
        public PassengerRatingEnum Rating { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
