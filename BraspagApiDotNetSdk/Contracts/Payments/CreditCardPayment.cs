using BraspagApiDotNetSdk.Contracts.Antifraud;
using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Contracts.Payments
{
	public class CreditCardPayment : Payment
	{
		public CreditCardPayment()
		{
			Type = "CreditCard";
		}
		
		public long ServiceTaxAmount { get; set; }

		public short Installments { get; set; }

		public InterestTypeEnum Interest { get; set; }

		public bool Capture { get; set; }

		public bool Authenticate { get; set; }

        public bool Recurrent { get; set; }

		public Card CreditCard { get; set; }

		public string AuthenticationUrl { get; set; }

		public string ProofOfSale { get; set; }
		
		public string AcquirerTransactionId { get; set; }
		
		public string AuthorizationCode { get; set; }

		public string SoftDescriptor { get; set; }

        public FraudAnalysis FraudAnalysis { get; set; }

        public ExternalAuthentication ExternalAuthentication { get; set; }

        public Card NewCard { get; set; }

        public Wallet Wallet { get; set; }
    }
}
