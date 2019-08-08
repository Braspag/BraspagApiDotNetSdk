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
using System.Globalization;
using System.Linq;
using System.Net;

namespace BraspagApiDotNetSdk.Tests
{
	[TestClass]
	public class VoidTests
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
		public void VoidSale_Send_Corrrectly_ApiService()
		{
			var paymentId = Guid.NewGuid();

			var validVoidSaleResponse = ValidVoidSaleResponse();

			_mockRestClient.Setup(m => m.Execute<VoidResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<VoidResponse>()
			{
				StatusCode = HttpStatusCode.OK,
				Content = new JsonSerializer().Serialize(validVoidSaleResponse),
				Data = validVoidSaleResponse
			});

			_service.Void(paymentId, MerchantAuthenticationHelper.CreateMerchantAuthentication(), VoidTransactionHelper.CreateValidVoidRequest());

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.IsAny<RestRequest>()), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Method == Method.PUT)), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Resource == @"sales/{paymentId}/void")), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
				param => param.Type == ParameterType.UrlSegment && param.Name == "paymentId" && param.Value.ToString() == paymentId.ToString()))), Times.Once);
		}

		[TestMethod]
		public void VoidSale_With_Opcional_Parameters_Send_Corrrectly_ApiService()
		{
			var paymentId = Guid.NewGuid();

			const int amount = 15057;

			var validVoidSaleResponse = ValidVoidSaleResponse();

			_mockRestClient.Setup(m => m.Execute<VoidResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<VoidResponse>()
			{
				StatusCode = HttpStatusCode.OK,
				Content = new JsonSerializer().Serialize(validVoidSaleResponse),
				Data = validVoidSaleResponse
			});

            _service.Void(paymentId, MerchantAuthenticationHelper.CreateMerchantAuthentication(), VoidTransactionHelper.CreateValidVoidRequest());

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.IsAny<RestRequest>()), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Method == Method.PUT)), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Resource == @"sales/{paymentId}/void")), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
				param => param.Type == ParameterType.UrlSegment && param.Name == "paymentId" && param.Value.ToString() == paymentId.ToString()))), Times.Once);

			_mockRestClient.Verify(m => m.Execute<VoidResponse>(It.Is<RestRequest>(request => request.Parameters.Any(
				param => param.Type == ParameterType.QueryString && param.Name == "amount" && param.Value.ToString() == amount.ToString(CultureInfo.InvariantCulture)))), Times.Once);
		}

		[TestMethod]
		public void VoidSale_Void_Transaction()
		{
			var validVoidResponse = ValidVoidSaleResponse();

			_mockRestClient.Setup(m => m.Execute<VoidResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<VoidResponse>()
			{
				StatusCode = HttpStatusCode.OK,
				Content = new JsonSerializer().Serialize(validVoidResponse),
				Data = validVoidResponse
			});

            var response = _service.Void(Guid.NewGuid(), MerchantAuthenticationHelper.CreateMerchantAuthentication(), VoidTransactionHelper.CreateValidVoidRequest());

			response.ErrorDataCollection.Should().BeNull();
			response.HttpStatus.Should().Be(HttpStatusCode.OK);
			response.ReasonCode.Should().Be(0);
			response.ReasonMessage.Should().Be("Successful");
			response.Status.Should().Be(10);
			response.Links.Count.Should().Be(1);
		}

		[TestMethod]
		public void VoidSale_Error_Transaction()
		{
			var errorVoidResponse = new VoidResponse
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

			_mockRestClient.Setup(m => m.Execute<VoidResponse>(It.IsAny<IRestRequest>())).Returns(new RestResponse<VoidResponse>()
			{
				StatusCode = HttpStatusCode.BadRequest,
				Content = new JsonSerializer().Serialize(errorVoidResponse),
				Data = errorVoidResponse
			});

			var response = _service.Void(Guid.NewGuid(), MerchantAuthenticationHelper.CreateMerchantAuthentication(), VoidTransactionHelper.CreateValidVoidRequest());

			response.ErrorDataCollection.Count.Should().BeGreaterThan(0);
			response.HttpStatus.Should().Be(HttpStatusCode.BadRequest);
			response.ReasonCode.Should().Be(0);
			response.ReasonMessage.Should().BeNull();
			response.Status.Should().Be(0);
			response.Links.Should().BeNull();
		}

		private static VoidResponse ValidVoidSaleResponse()
		{
			return new VoidResponse
			{
				HttpStatus = HttpStatusCode.OK,
				Links = new List<Link>()
				{
					new Link
					{
						Href = "self",
						Method = "GET",
						Rel = "https://apisandbox.braspag.com.br/v2/sales/" + Guid.NewGuid()
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
