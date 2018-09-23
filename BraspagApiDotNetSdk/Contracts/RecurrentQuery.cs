namespace BraspagApiDotNetSdk.Contracts
{
    public class RecurrentQuery : BaseResponse
    {
        public Customer Customer { get; set; }
        public RecurrentPaymentQueryModel RecurrentPayment { get; set; }
    }
}
