namespace BraspagApiDotNetSdk.Contracts.Antifraud
{
    public class ReplyData
    {
        public string AddressInfoCode { get; set; }
        public string FactorCode { get; set; }
        public int Score { get; set; }
        public string BinCountry { get; set; }
        public string CardIssuer { get; set; }
        public string CardScheme { get; set; }
        public int HostSeverity { get; set; }
        public string HotListInfoCode { get; set; }
        public string IdentityInfoCode { get; set; }
        public string InternetInfoCode { get; set; }
        public string IpCity { get; set; }
        public string IpCountry { get; set; }
        public string IpRoutingMethod { get; set; }
        public string IpState { get; set; }
        public string PhoneInfoCode { get; set; }
        public string ScoreModelUsed { get; set; }
        public string VelocityInfoCode { get; set; }
        public int? CasePriority { get; set; }
        public FingerPrintData FingerPrint { get; set; }
    }
}
