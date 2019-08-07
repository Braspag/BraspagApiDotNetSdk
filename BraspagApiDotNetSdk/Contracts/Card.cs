using BraspagApiDotNetSdk.Contracts.Enum;
using System;

namespace BraspagApiDotNetSdk.Contracts
{
	public class Card
	{
		public string CardNumber { get; set; }

		public string Holder { get; set; }

		public string ExpirationDate { get; set; }

		public string SecurityCode { get; set; }

		public bool SaveCard { get; set; }

		public Guid? CardToken { get; set; }

        public string Cryptogram { get; set; }

        public string Alias { get; set; }

		public BrandEnum Brand { get; set; }

        public Avs Avs { get; set; }
    }
}
