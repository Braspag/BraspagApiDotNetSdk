using BraspagApiDotNetSdk.Contracts.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BraspagApiDotNetSdk.Contracts
{
    public class VerifyCardRequest
    {
        public Card Card { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ProviderEnum Provider { get; set; }
    }
}
