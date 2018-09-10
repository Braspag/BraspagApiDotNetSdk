namespace BraspagApiDotNetSdk.Contracts.Payments
{
	public class BoletoPayment : Payment
	{
		public BoletoPayment()
		{
			Type = "Boleto";
		}

		public string Instructions { get; set; }

		public string ExpirationDate { get; set; }

		public string Demostrative { get; set; }

		public string Url { get; set; }

		public string BoletoNumber { get; set; }

		public string BarCodeNumber { get; set; }

		public string DigitableLine { get; set; }

		public string Assignor { get; set; }

		public string Address { get; set; }

		public string Identification { get; set; }

        public bool IsRecurring { get; set; }
    }
}
