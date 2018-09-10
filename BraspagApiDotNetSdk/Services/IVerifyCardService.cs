using BraspagApiDotNetSdk.Contracts;

namespace BraspagApiDotNetSdk.Services
{
    public interface IVerifyCardService
    {
        VerifyCardResponse VerifyCard(MerchantAuthentication merchantAuthentication, VerifyCardRequest verifyCardRequest);
    }
}
