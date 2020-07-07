using BraspagApiDotNetSdk.Contracts.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace BraspagApiDotNetSdk.Contracts
{
    public class Wallet
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public WalletTypeEnum Type { get; set; }

        public string WalletKey { get; set; }
        public string Cavv { get; set; }
        public short Eci { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }
    }
}
