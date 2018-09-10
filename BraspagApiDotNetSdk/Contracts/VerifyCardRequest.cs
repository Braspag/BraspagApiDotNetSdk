using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Contracts
{
    public class VerifyCardRequest
    {
        public Card Card { get; set; }
        public ProviderEnum Provider { get; set; }
    }
}
