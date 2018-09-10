using BraspagApiDotNetSdk.Contracts;
using System;
using System.Collections.Generic;

namespace BraspagApiDotNetSdk.Services
{
    public interface IPagadorApiService
    {
        Sale CreateSale(MerchantAuthentication merchantAuthentication, Sale sale);

        CaptureResponse Capture(Guid paymentId, MerchantAuthentication merchantAuthentication, CaptureRequest captureRequest);

        VoidResponse Void(Guid paymentId, MerchantAuthentication merchantAuthentication, VoidRequest voidRequest);

        Sale Get(Guid paymentId, MerchantAuthentication merchantAuthentication);

        Sale CreateSale(MerchantAuthentication merchantAuthentication, Sale sale, Dictionary<string, string> headers);
    }
}