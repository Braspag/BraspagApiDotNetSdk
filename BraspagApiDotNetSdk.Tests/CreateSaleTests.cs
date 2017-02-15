using BraspagApiDotNetSdk.Contracts;
using BraspagApiDotNetSdk.Services;
using BraspagApiDotNetSdk.Tests.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BraspagApiDotNetSdk.Tests
{
    [TestClass]
    public class CreateSaleTests
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
        public void CreateSale_Send_Corrrectly_ApiService()
        {
            var validCreditCardSaleResponse = ValidCreateSaleResponse(CardTransactionHelper.CreateCreditCardPaymentResponse());

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new JsonSerializer().Serialize(validCreditCardSaleResponse),
                Data = validCreditCardSaleResponse
            });

            _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(CardTransactionHelper.CreateCreditCardPaymentRequest()));

            _mockRestClient.Verify(m => m.Execute<Sale>(It.IsAny<RestRequest>()), Times.Once);

            _mockRestClient.Verify(m => m.Execute<Sale>(It.Is<RestRequest>(request => request.Method == Method.POST)), Times.Once);

            _mockRestClient.Verify(m => m.Execute<Sale>(It.Is<RestRequest>(request => request.Resource == @"sales")), Times.Once);

            _mockRestClient.Verify(m => m.Execute<Sale>(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.RequestBody && param.Name == @"application/json"))), Times.Once);
        }

        [TestMethod]
        public void CreateSale_Send_CreditCardTransaction_Return_Valid_Reponse()
        {
            var validCreditCardSaleResponse = ValidCreateSaleResponse(CardTransactionHelper.CreateCreditCardPaymentResponse());

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new JsonSerializer().Serialize(validCreditCardSaleResponse),
                Data = validCreditCardSaleResponse
            });

            var result = _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(CardTransactionHelper.CreateCreditCardPaymentRequest()));

            result.Should().NotBeNull();
            result.MerchantOrderId.Should().NotBeEmpty();
            result.Customer.Should().NotBeNull();
            result.Customer.Address.Should().NotBeNull();
            result.Payment.Should().NotBeNull();
            result.Payment.ExtraDataCollection.Should().NotBeNull();
            result.Payment.Links.Should().NotBeNull();

            result.Customer.Address.City.Should().Be(CustomerHelper.CreateCustomer().Address.City);
            result.Customer.Address.Complement.Should().Be(CustomerHelper.CreateCustomer().Address.Complement);
            result.Customer.Address.Country.Should().Be(CustomerHelper.CreateCustomer().Address.Country);
            result.Customer.Address.District.Should().Be(CustomerHelper.CreateCustomer().Address.District);
            result.Customer.Address.Number.Should().Be(CustomerHelper.CreateCustomer().Address.Number);
            result.Customer.Address.State.Should().Be(CustomerHelper.CreateCustomer().Address.State);
            result.Customer.Address.Street.Should().Be(CustomerHelper.CreateCustomer().Address.Street);
            result.Customer.Address.ZipCode.Should().Be(CustomerHelper.CreateCustomer().Address.ZipCode);

            result.Customer.Birthdate.Should().NotBe(new DateTime());
            result.Customer.Email.Should().Be(CustomerHelper.CreateCustomer().Email);
            result.Customer.Identity.Should().Be(CustomerHelper.CreateCustomer().Identity);
            result.Customer.IdentityType.Should().Be(CustomerHelper.CreateCustomer().IdentityType);
            result.Customer.Name.Should().Be(CustomerHelper.CreateCustomer().Name);

            result.Payment.Amount.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().Amount);
            result.Payment.CapturedAmount.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().CapturedAmount);
            result.Payment.Provider.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().Provider);
            result.Payment.Country.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().Country);
            result.Payment.Credentials.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().Credentials);
            result.Payment.Currency.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().Currency);
            result.Payment.ReasonCode.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().ReasonCode);
            result.Payment.ReasonMessage.Should().Be("Successful");
            result.Payment.ReturnUrl.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().ReturnUrl);
            result.Payment.Status.Should().Be(1);
            result.Payment.Type.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().Type);
            result.Payment.VoidedAmount.Should().Be(CardTransactionHelper.CreateCreditCardPaymentResponse().VoidedAmount);
        }

        [TestMethod]
        public void CreateSale_Send_BoletoTransaction_Return_Valid_Reponse()
        {
            var validCreditCardSaleResponse = ValidCreateSaleResponse(BoletoTransactionHelper.BoletoTransactionResponse());

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new JsonSerializer().Serialize(validCreditCardSaleResponse),
                Data = validCreditCardSaleResponse
            });

            var response = _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(BoletoTransactionHelper.BoletoTransactionRequest()));

            response.Should().NotBeNull();
            response.MerchantOrderId.Should().NotBeEmpty();
            response.Customer.Should().NotBeNull();
            response.Customer.Address.Should().NotBeNull();
            response.Payment.Should().NotBeNull();
            response.Payment.ExtraDataCollection.Should().BeNull();
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
            response.Customer.IdentityType.Should().Be(CustomerHelper.CreateCustomer().IdentityType);
            response.Customer.Name.Should().Be(CustomerHelper.CreateCustomer().Name);

            response.Payment.Amount.Should().Be(BoletoTransactionHelper.BoletoTransactionRequest().Amount);
            response.Payment.CapturedAmount.Should().Be(null);
            response.Payment.Country.Should().Be(BoletoTransactionHelper.BoletoTransactionRequest().Country);
            response.Payment.Currency.Should().Be(BoletoTransactionHelper.BoletoTransactionRequest().Currency);
            response.Payment.PaymentId.Should().NotBeEmpty();
            response.Payment.ReasonCode.Should().Be(0);
            response.Payment.ReasonMessage.Should().Be("Successful");
            response.Payment.Status.Should().Be(1);
            response.Payment.Type.Should().Be("Boleto");
        }

        [TestMethod]
        public void CreateSale_Send_DebitCardTransaction_Return_Valid_Reponse()
        {
            var validDebitCardSaleResponse = ValidCreateSaleResponse(CardTransactionHelper.CreateDebitCardPaymentResponse());

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new JsonSerializer().Serialize(validDebitCardSaleResponse),
                Data = validDebitCardSaleResponse
            });

            var response = _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(CardTransactionHelper.CreateDebitCardPaymentRequest()));

            response.Should().NotBeNull();
            response.MerchantOrderId.Should().NotBeEmpty();
            response.Customer.Should().NotBeNull();
            response.Customer.Address.Should().NotBeNull();
            response.Payment.Should().NotBeNull();
            response.Payment.ExtraDataCollection.Should().BeNull();
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
            response.Customer.IdentityType.Should().Be(CustomerHelper.CreateCustomer().IdentityType);
            response.Customer.Name.Should().Be(CustomerHelper.CreateCustomer().Name);

            response.Payment.Amount.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Amount);
            response.Payment.CapturedAmount.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().CapturedAmount);
            response.Payment.Provider.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Provider);
            response.Payment.Country.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Country);
            response.Payment.Credentials.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Credentials);
            response.Payment.Currency.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Currency);
            response.Payment.ReasonCode.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().ReasonCode);
            response.Payment.ReasonMessage.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().ReasonMessage);
            response.Payment.ReturnUrl.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().ReturnUrl);
            response.Payment.Status.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Status);
            response.Payment.Type.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().Type);
            response.Payment.VoidedAmount.Should().Be(CardTransactionHelper.CreateDebitCardPaymentResponse().VoidedAmount);
        }

        [TestMethod]
        public void CreateSale_Send_EletronicTransferTransaction_Return_Valid_Reponse()
        {
            var validEletronicTransferSaleResponse = ValidCreateSaleResponse(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse());

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
            {
                StatusCode = HttpStatusCode.Created,
                Content = new JsonSerializer().Serialize(validEletronicTransferSaleResponse),
                Data = validEletronicTransferSaleResponse
            });

            var response = _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(EletronicTransferTransactionHelper.EletronicTransferTransactionRequest()));

            response.Should().NotBeNull();
            response.MerchantOrderId.Should().NotBeEmpty();
            response.Customer.Should().NotBeNull();
            response.Customer.Address.Should().NotBeNull();
            response.Payment.Should().NotBeNull();
            response.Payment.ExtraDataCollection.Should().BeNull();
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
            response.Customer.IdentityType.Should().Be(CustomerHelper.CreateCustomer().IdentityType);
            response.Customer.Name.Should().Be(CustomerHelper.CreateCustomer().Name);

            response.Payment.Amount.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Amount);
            response.Payment.CapturedAmount.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().CapturedAmount);
            response.Payment.Provider.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Provider);
            response.Payment.Country.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Country);
            response.Payment.Credentials.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Credentials);
            response.Payment.Currency.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Currency);
            response.Payment.ReasonCode.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().ReasonCode);
            response.Payment.ReasonMessage.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().ReasonMessage);
            response.Payment.ReturnUrl.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().ReturnUrl);
            response.Payment.Status.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Status);
            response.Payment.Type.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().Type);
            response.Payment.VoidedAmount.Should().Be(EletronicTransferTransactionHelper.EletronicTransferTransactionResponse().VoidedAmount);
        }

        [TestMethod]
        public void CreateSale_Send_CreditCardTransaction_Return_Error_Reponse()
        {
            var validCreditCardSaleResponse = ErrorCreateSaleResponse();

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(new RestResponse<Sale>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new JsonSerializer().Serialize(validCreditCardSaleResponse),
                Data = validCreditCardSaleResponse
            });

            var response = _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(CardTransactionHelper.CreateCreditCardPaymentRequest()));

            response.Should().NotBeNull();
            response.MerchantOrderId.Should().BeNullOrEmpty();
            response.Customer.Should().BeNull();
            response.Payment.Should().BeNull();
            response.HttpStatus.Should().Be(HttpStatusCode.BadRequest);
            response.ErrorDataCollection.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void CreateSale_WhenHttpStatusCodeEqualsTo0_ReturnsMappedErrorResponse()
        {
            var restResponse = new RestResponse<Sale>
            {
                StatusCode = 0,
                ErrorException = new Exception("Exceção de teste"),
                ErrorMessage = "Undefined Error"
            };

            _mockRestClient.Setup(m => m.Execute<Sale>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var response = _service.CreateSale(MerchantAuthenticationHelper.CreateMerchantAuthentication(), SaleHelper.CreateOrder(CardTransactionHelper.CreateCreditCardPaymentRequest()));

            response.Should().NotBeNull();
            response.MerchantOrderId.Should().BeNullOrEmpty();
            response.Customer.Should().BeNull();
            response.Payment.Should().BeNull();
            Assert.AreEqual(response.HttpStatus, restResponse.StatusCode);
            response.ErrorDataCollection.Should().NotBeNull();
            response.ErrorDataCollection[0].Code.Should().Be(-1);
            response.ErrorDataCollection[0].Message.Should().Be("ErrorMessage: Undefined Error | ErrorException: System.Exception: Exceção de teste");
        }

        private static Sale ValidCreateSaleResponse(Payment payment)
        {
            return new Sale
            {
                Customer = CustomerHelper.CreateCustomer(),
                HttpStatus = HttpStatusCode.OK,
                MerchantOrderId = Guid.NewGuid().ToString(),
                Payment = payment,
                ErrorDataCollection = null
            };
        }

        private static Sale ErrorCreateSaleResponse()
        {
            return new Sale
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
        }
    }
}