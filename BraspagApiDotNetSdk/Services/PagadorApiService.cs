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
    public class PagadorApiService : IPagadorApiService
    {
        public IRestClient RestClient { get; set; }

        protected IDeserializer JsonDeserializer { get; set; }

        public PagadorApiService() : this(ConfigurationManager.AppSettings["apiRootUrl"])
        {
        }

        public PagadorApiService(string url)
        {
            RestClient = new RestClient { BaseUrl = new Uri(url) };
            JsonDeserializer = new JsonDeserializer();
        }

        public Sale CreateSale(MerchantAuthentication merchantAuthentication, Sale sale)
        {
            var restRequest = new RestRequest(@"sales", Method.POST) { RequestFormat = DataFormat.Json };
            AddHeaders(restRequest, merchantAuthentication);

            return PostSale(sale, restRequest);
        }

        public Sale CreateSale(MerchantAuthentication merchantAuthentication, Sale sale, Dictionary<string, string> headers)
        {
            var restRequest = new RestRequest(@"sales", Method.POST) { RequestFormat = DataFormat.Json };
            AddHeaders(restRequest, merchantAuthentication);
            AddCustomHeaders(restRequest, headers ?? new Dictionary<string, string>());

            return PostSale(sale, restRequest);
        }

        public CaptureResponse Capture(Guid paymentId, MerchantAuthentication merchantAuthentication, CaptureRequest captureRequest)
        {
            var restRequest = new RestRequest(@"sales/{paymentId}/capture", Method.PUT) { RequestFormat = DataFormat.Json };
            AddHeaders(restRequest, merchantAuthentication);

            restRequest.AddUrlSegment("paymentId", paymentId.ToString());

            if (captureRequest != null)
            {
                restRequest.AddQueryParameter("amount", captureRequest.Amount.ToString());
                restRequest.AddQueryParameter("serviceTaxAmount", captureRequest.ServiceTaxAmount.ToString());
            }

            var response = RestClient.Execute<CaptureResponse>(restRequest);

            CaptureResponse captureResponse = null;

            if (response.StatusCode == HttpStatusCode.OK)
                captureResponse = JsonConvert.DeserializeObject<CaptureResponse>(response.Content);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                captureResponse = new CaptureResponse { ErrorDataCollection = JsonDeserializer.Deserialize<List<Error>>(response) };
            else
                captureResponse = new CaptureResponse();

            captureResponse.HttpStatus = response.StatusCode;

            return captureResponse;
        }

        public VoidResponse Void(Guid paymentId, MerchantAuthentication merchantAuthentication, VoidRequest voidRequest)
        {
            var restRequest = new RestRequest(@"sales/{paymentId}/void", Method.PUT) { RequestFormat = DataFormat.Json };
            AddHeaders(restRequest, merchantAuthentication);

            restRequest.AddUrlSegment("paymentId", paymentId.ToString());

            if (voidRequest.Amount != null)
            {
                restRequest.AddQueryParameter("amount", voidRequest.Amount.ToString());
            }

            var response = RestClient.Execute<VoidResponse>(restRequest);

            VoidResponse voidResponse = null;

            if (response.StatusCode == HttpStatusCode.OK)
                voidResponse = JsonConvert.DeserializeObject<VoidResponse>(response.Content);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                voidResponse = new VoidResponse { ErrorDataCollection = JsonDeserializer.Deserialize<List<Error>>(response) };
            else
                voidResponse = new VoidResponse();

            voidResponse.HttpStatus = response.StatusCode;

            return voidResponse;
        }

        public Sale Get(Guid paymentId, MerchantAuthentication merchantAuthentication)
        {
            var restRequest = new RestRequest(@"sales/{paymentId}", Method.GET) { RequestFormat = DataFormat.Json };
            AddHeaders(restRequest, merchantAuthentication);

            restRequest.AddUrlSegment("paymentId", paymentId.ToString());

            var response = RestClient.Execute<Sale>(restRequest);

            Sale saleResponse = null;

            if (response.StatusCode == HttpStatusCode.OK)
                saleResponse = JsonConvert.DeserializeObject<Sale>(response.Content);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                saleResponse = new Sale { ErrorDataCollection = JsonDeserializer.Deserialize<List<Error>>(response) };
            else
                saleResponse = new Sale();

            saleResponse.HttpStatus = response.StatusCode;

            return saleResponse;
        }

        private static void AddHeaders(IRestRequest request, MerchantAuthentication auth)
        {
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("MerchantId", auth.MerchantId.ToString());
            request.AddHeader("MerchantKey", auth.MerchantKey);
        }

        private static void AddCustomHeaders(IRestRequest request, Dictionary<string, string> customHeaders)
        {
            foreach (var header in customHeaders)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        private static Sale HandleErrorException(IRestResponse<Sale> response)
        {
            return new Sale
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

        private Sale PostSale(Sale sale, RestRequest restRequest)
        {
            restRequest.AddBody(sale);

            var response = RestClient.Execute<Sale>(restRequest);

            Sale saleResponse = null;

            if (response.StatusCode == HttpStatusCode.Created)
                saleResponse = JsonConvert.DeserializeObject<Sale>(response.Content);
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                saleResponse = new Sale { ErrorDataCollection = JsonDeserializer.Deserialize<List<Error>>(response) };
            else if (response.StatusCode == 0)
            {
                saleResponse = HandleErrorException(response);
            }
            else
                saleResponse = new Sale();

            saleResponse.HttpStatus = response.StatusCode;

            return saleResponse;
        }
    }
}