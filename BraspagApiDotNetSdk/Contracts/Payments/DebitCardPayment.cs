namespace BraspagApiDotNetSdk.Contracts.Payments
{
	public class DebitCardPayment : Payment
	{
		public DebitCardPayment()
		{
			Type = "DebitCard";
		}

		public Card DebitCard { get; set; }

		public string AuthenticationUrl { get; set; }

		public string ProofOfSale { get; set; }
		
		public string AcquirerTransactionId { get; set; }
		
		public string AuthorizationCode { get; set; }

		public string SoftDescriptor { get; set; }

        public string Eci { get; set; }

        public string IssuerTransactionId { get; set; }
    }
}
