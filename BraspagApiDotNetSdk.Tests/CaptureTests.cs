using BraspagApiDotNetSdk.Contracts;
using BraspagApiDotNetSdk.Services;
using BraspagApiDotNetSdk.Tests.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BraspagApiDotNetSdk.Tests
{
    [TestClass]
    public class CaptureTests
    {
        private PagadorApiService _service;

        private Mock<IRestClient> _mockRestClient;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRestClient = new Mock<IRestClient>();

            _service = new PagadorApiService
            {
                RestClient = _mockRestClient.Object
            };
        }

        [TestMethod]
        public void CaptureSale_Send_Corrrectly_ApiService()
        {
            var paymentId = Guid.NewGuid();

            var validCaptureSaleResponse = ValidCaptureSaleResponse();

            _mockRestClient.Setup(m => m.Execute<CaptureResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<CaptureResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonSerializer().Serialize(validCaptureSaleResponse),
                Data = validCaptureSaleResponse
            });

            _service.Capture(paymentId, MerchantAuthenticationHelper.CreateMerchantAuthentication(), new CaptureRequest());

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.IsAny<RestRequest>()), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Method == Method.PUT)), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Resource == @"sales/{paymentId}/capture")), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.UrlSegment && param.Name == "paymentId" && param.Value.ToString() == paymentId.ToString()))), Times.Once);
        }

        [TestMethod]
        public void CaptureSale_With_Opcional_Parameters_Send_Corrrectly_ApiService()
        {
            var paymentId = Guid.NewGuid();

            var validCaptureSaleResponse = ValidCaptureSaleResponse();

            _mockRestClient.Setup(m => m.Execute<CaptureResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<CaptureResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonSerializer().Serialize(validCaptureSaleResponse),
                Data = validCaptureSaleResponse
            });

            _service.Capture(paymentId, MerchantAuthenticationHelper.CreateMerchantAuthentication(), CaptureTransationHelper.CreateValidCaptureRequest());

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.IsAny<RestRequest>()), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Method == Method.PUT)), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Resource == @"sales/{paymentId}/capture")), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.UrlSegment && param.Name == "paymentId" && param.Value.ToString() == paymentId.ToString()))), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.QueryString && param.Name == "amount" && param.Value.ToString() == CaptureTransationHelper.CreateValidCaptureRequest().Amount.ToString()))), Times.Once);

            _mockRestClient.Verify(m => m.Execute<CaptureResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.QueryString && param.Name == "serviceTaxAmount" && param.Value.ToString() == CaptureTransationHelper.CreateValidCaptureRequest().ServiceTaxAmount.ToString()))), Times.Once);
        }

        [TestMethod]
        public void CaptureSale_Capture_Valid_Transaction()
        {
            var validCreditCardCaptureResponse = ValidCaptureSaleResponse();

            _mockRestClient.Setup(m => m.Execute<CaptureResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<CaptureResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonSerializer().Serialize(validCreditCardCaptureResponse),
                Data = validCreditCardCaptureResponse
            });

            _service.RestClient = _mockRestClient.Object;

            var response = _service.Capture(Guid.NewGuid(), MerchantAuthenticationHelper.CreateMerchantAuthentication(), CaptureTransationHelper.CreateValidCaptureRequest());

            response.ErrorDataCollection.Should().BeNull();
            response.HttpStatus.Should().Be(HttpStatusCode.OK);
            response.ReasonCode.Should().Be(0);
            response.ReasonMessage.Should().Be("Successful");
            response.Status.Should().Be(10);
            response.Links.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void CaptureSale_Error_Transaction()
        {
            var errorCreditCardCaptureResponse = new CaptureResponse
            {
                HttpStatus = HttpStatusCode.BadRequest,
                Links = null,
                ReasonCode = 0,
                Status = 0,
                ReasonMessage = null,
                ErrorDataCollection = new List<Error>()
                {
                    new Error
                    {
                        Code = 500,
                        Message = "Internal Server Error"
                    }
                }
            };

            _mockRestClient.Setup(m => m.Execute<CaptureResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<CaptureResponse>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new JsonSerializer().Serialize(errorCreditCardCaptureResponse),
                Data = errorCreditCardCaptureResponse
            });

            var response = _service.Capture(Guid.NewGuid(), MerchantAuthenticationHelper.CreateMerchantAuthentication(), CaptureTransationHelper.CreateValidCaptureRequest());

            response.ErrorDataCollection.Count.Should().BeGreaterThan(0);
            response.HttpStatus.Should().Be(HttpStatusCode.BadRequest);
            response.ReasonCode.Should().Be(0);
            response.ReasonMessage.Should().BeNull();
            response.Status.Should().Be(0);
            response.Links.Should().BeNull();
        }

        private static CaptureResponse ValidCaptureSaleResponse()
        {
            return new CaptureResponse
            {
                HttpStatus = HttpStatusCode.OK,
                Links = new List<Link>()
                {
                    new Link
                    {
                        Href = "self",
                        Method = "GET",
                        Rel = "https://apisandbox.pagador.com.br/v2/sales/" + Guid.NewGuid()
                    }
                },
                ReasonCode = 0,
                Status = 10,
                ReasonMessage = "Successful",
                ErrorDataCollection = null
            };
        }
    }
}