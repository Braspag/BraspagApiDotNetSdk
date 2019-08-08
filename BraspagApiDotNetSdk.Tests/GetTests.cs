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
	public class GetTests
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
		public void GetSale_Send_Corrrectly_ApiService()
		{
			var paymentId = Guid.NewGuid();
			
			var validGetSaleResponse = ValidGetSaleResponse();

			_mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
			{
				StatusCode = HttpStatusCode.OK,
				Content = new JsonSerializer().Serialize(validGetSaleResponse),
				Data = validGetSaleResponse
			});

			_service.Get(paymentId, MerchantAuthenticationHelper.CreateMerchantAuthentication());

			_mockRestClient.Verify(m => m.Execute<Sale>(It.IsAny<RestRequest>()), Times.Once);

			_mockRestClient.Verify(m => m.Execute<Sale>(It.Is<RestRequest>(request => request.Method == Method.GET)), Times.Once);

			_mockRestClient.Verify(m => m.Execute<Sale>(It.Is<RestRequest>(request => request.Resource == @"sales/{paymentId}")), Times.Once);

			_mockRestClient.Verify(m => m.Execute<Sale>(It.Is<RestRequest>(request => request.Parameters.Any(
				param => param.Type == ParameterType.UrlSegment && param.Name == "paymentId" && param.Value.ToString() == paymentId.ToString()))), Times.Once);
		}

		[TestMethod]
		public void GetSale_Get_Transaction()
		{
			var validSaleResponse = ValidGetSaleResponse();

			_mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
			{
				StatusCode = HttpStatusCode.OK,
				Content = new JsonSerializer().Serialize(validSaleResponse),
				Data = validSaleResponse
			});

			var response = _service.Get(validSaleResponse.Payment.PaymentId, MerchantAuthenticationHelper.CreateMerchantAuthentication());

			response.Should().NotBeNull();
			response.MerchantOrderId.Should().NotBeEmpty();
			response.Customer.Should().NotBeNull();
			response.Customer.Address.Should().NotBeNull();
			response.Payment.Should().NotBeNull();
			response.Payment.ExtraDataCollection.Should().NotBeNull();
			response.Payment.Links.Should().NotBeNull();

			response.Customer.Address.City.Should().Be(CustomerHelper.CreateCustomer().Address.City);
			response.Customer.Address.Complement.Should().Be(CustomerHelper.CreateCustomer().Address.Complement);
			response.Customer.Address.Country.Should().Be(CustomerHelper.CreateCustomer().Address.Country);
			response.Customer.Address.District.Should().Be(CustomerHelper.CreateCustomer().Address.District);
			response.Customer.Address.Number.Should().Be(CustomerHelper.CreateCustomer().Address.Number);
			response.Customer.Address.State.Should().Be(CustomerHelper.CreateCustomer().Address.State);
			response.Customer.Address.Street.Should().Be(CustomerHelper.CreateCustomer().Address.Street);
			response.Customer.Address.ZipCode.Should().Be(CustomerHelper.CreateCustomer().Address.ZipCode);

			response.Customer.Birthdate.Should().NotBe(new DateTime());
			response.Customer.Email.Should().Be(CustomerHelper.CreateCustomer().Email);
			response.Customer.Identity.Should().Be(CustomerHelper.CreateCustomer().Identity);
			response.Customer.IdentityType.Should().Be(null);
			response.Customer.Name.Should().Be(CustomerHelper.CreateCustomer().Name);

			response.Payment.Amount.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().Amount);
			response.Payment.CapturedAmount.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().CapturedAmount);
			response.Payment.Provider.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().Provider);
			response.Payment.Country.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().Country);
			response.Payment.Credentials.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().Credentials);
			response.Payment.Currency.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().Currency);
			response.Payment.ReasonCode.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().ReasonCode);
			response.Payment.ReasonMessage.Should().Be("Successful");
			response.Payment.ReturnUrl.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().ReturnUrl);
			response.Payment.Status.Should().Be(1);
			response.Payment.Type.Should().Be(CardTransactionHelper.CreateCreditCardPaymentRequest().Type);
		}

		[TestMethod]
		public void GetSale_Send_CreditCardTransaction_Return_Error_Reponse()
		{
			var errorResponse = new Sale
			{
				Customer = null,
				HttpStatus = HttpStatusCode.BadRequest,
				MerchantOrderId = null,
				Payment = null,
				ErrorDataCollection = new List<Error>()
				{
					new Error
					{
						Code = 500,
						Message = "Internal Server Error"
					}
				}
			};

			_mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
			{
				StatusCode = HttpStatusCode.BadRequest,
				Content = new JsonSerializer().Serialize(errorResponse),
				Data = errorResponse
			});

			var response = _service.Get(Guid.NewGuid(), MerchantAuthenticationHelper.CreateMerchantAuthentication());

			response.Should().NotBeNull();
			response.MerchantOrderId.Should().BeNullOrEmpty();
			response.Customer.Should().BeNull();
			response.Payment.Should().BeNull();
			response.HttpStatus.Should().Be(HttpStatusCode.BadRequest);
			response.ErrorDataCollection.Count.Should().BeGreaterThan(0);
		}

		private static Sale ValidGetSaleResponse()
		{
			var sale = SaleHelper.CreateOrder(CardTransactionHelper.CreateCreditCardPaymentRequest());

			sale.Customer.IdentityType = null;

			sale.Payment = CardTransactionHelper.CreateCreditCardPaymentResponse();

			return sale;
		}
	}
}
