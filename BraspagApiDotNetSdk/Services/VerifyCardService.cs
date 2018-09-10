using BraspagApiDotNetSdk.Contracts;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace BraspagApiDotNetSdk.Services
{
    public class VerifyCardService : IVerifyCardService
    {
        public IRestClient RestClient { get; set; }

        protected IDeserializer JsonDeserializer { get; set; }

        public VerifyCardService() : this(ConfigurationManager.AppSettings["apiRootUrl"]) { }

        public VerifyCardService(string url)
        {
            RestClient = new RestClient(new Uri(url));
            JsonDeserializer = new JsonDeserializer();
        }

        public VerifyCardResponse VerifyCard(MerchantAuthentication merchantAuthentication, VerifyCardRequest verifyCardRequest)
        {
            var restRequest = new RestRequest(@"VerifyCard", Method.POST) { RequestFormat = DataFormat.Json };
            AddHeaders(restRequest, merchantAuthentication);

            restRequest.AddBody(verifyCardRequest);
            
            var response = RestClient.Execute(restRequest);
            
            VerifyCardResponse verifyCardResponse = null;

            if (response.StatusCode == HttpStatusCode.Created ||
                response.StatusCode == HttpStatusCode.Accepted ||
                response.StatusCode == HttpStatusCode.OK)

                verifyCardResponse = JsonConvert.DeserializeObject<VerifyCardResponse>(response.Content);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                verifyCardResponse = new VerifyCardResponse { ErrorDataCollection = JsonDeserializer.Deserialize<List<Error>>(response) };
            else if (response.StatusCode == 0)
            {
                verifyCardResponse = HandleErrorException(response);
            }
            else
                verifyCardResponse = new VerifyCardResponse();

            verifyCardResponse.HttpStatus = response.StatusCode;

            return verifyCardResponse;
        }

        private static void AddHeaders(IRestRequest request, MerchantAuthentication merchantAuthentication)
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("MerchantId", merchantAuthentication.MerchantId.ToString());
            request.AddHeader("MerchantKey", merchantAuthentication.MerchantKey);
        }

        private static VerifyCardResponse HandleErrorException(IRestResponse response)
        {
            return new VerifyCardResponse
            {
                ErrorDataCollection = new List<Error>
                {
                    new Error
                    {
                        Code = -1,
                        Message = string.Format("ErrorMessage: {0} | ErrorException: {1}", response.ErrorMessage, response.ErrorException)
                    }
                }
            };
        }
    }
}
