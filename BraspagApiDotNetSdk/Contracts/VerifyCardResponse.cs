using Newtonsoft.Json;

namespace BraspagApiDotNetSdk.Contracts
{
    public class VerifyCardResponse : BaseResponse
    {
        [JsonProperty(PropertyName = "Status")]
        public byte Status { get; set; }
        [JsonProperty(PropertyName = "ProviderReturnCode")]
        public string ProviderReturnCode { get; set; }
        [JsonProperty(PropertyName = "ProviderReturnMessage")]
        public string ProviderReturnMessage { get; set; }
    }
}
