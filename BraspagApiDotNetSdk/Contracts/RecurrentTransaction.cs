using System;

namespace BraspagApiDotNetSdk.Contracts
{
    public class RecurrentTransaction
    {
        public short PaymentNumber { get; set; }
        public Guid RecurrentPaymentId { get; set; }
        public Guid TransactionId { get; set; }
        public byte TryNumber { get; set; }
    }
}
