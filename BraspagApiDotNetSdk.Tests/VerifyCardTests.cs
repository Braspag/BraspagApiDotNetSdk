using BraspagApiDotNetSdk.Contracts;
using BraspagApiDotNetSdk.Services;
using BraspagApiDotNetSdk.Tests.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace BraspagApiDotNetSdk.Tests
{
    [TestClass]
    public class VerifyCardTests
    {
        private Mock<IRestClient> _mockRestClient;

        private IVerifyCardService _verifyCardService;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRestClient = new Mock<IRestClient>();

            _verifyCardService = new VerifyCardService
            {
                RestClient = _mockRestClient.Object
            };
        }

        [TestMethod]
        public void VerifyCard_WhenVerifyIsSuccessAndCardIsOk_ShouldFillResponseCorrectly()
        {
            var verifyCardRequest = VerifyCardHelper.ValidVerifyCardRequest();
            
            _mockRestClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(new RestResponse<string>
            {
                StatusCode = HttpStatusCode.Created,
                Content = VerifyCardHelper.ValidVerifyCardResponseWithTrueResponse(),
                Data = VerifyCardHelper.ValidVerifyCardResponseWithTrueResponse()
            });

            var response = _verifyCardService.VerifyCard(new MerchantAuthentication(), verifyCardRequest);

            _mockRestClient.Verify(m => m.Execute(It.IsAny<RestRequest>()), Times.Once);

            _mockRestClient.Verify(m => m.Execute(It.Is<RestRequest>(request => request.Method == Method.POST)), Times.Once);
            
            _mockRestClient.Verify(m => m.Execute(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.RequestBody && param.ContentType == @"application/json"))), Times.Once);

            response.Should().NotBeNull();
            response.ErrorDataCollection.Should().BeNull();
            response.Status.Should().Equals(1);
        }

        [TestMethod]
        public void VerifyCard_WhenVerifyIsSuccessAndCardIsKo_ShouldFillResponseCorrectly()
        {
            var verifyCardRequest = VerifyCardHelper.InvalidVerifyCardRequest();

            _mockRestClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(new RestResponse<string>
            {
                StatusCode = HttpStatusCode.Created,
                Content = VerifyCardHelper.ValidVerifyCardResponseWithFalseResponse(),
                Data = VerifyCardHelper.ValidVerifyCardResponseWithFalseResponse()
            });

            var response = _verifyCardService.VerifyCard(new MerchantAuthentication(), verifyCardRequest);

            _mockRestClient.Verify(m => m.Execute(It.IsAny<RestRequest>()), Times.Once);

            _mockRestClient.Verify(m => m.Execute(It.Is<RestRequest>(request => request.Method == Method.POST)), Times.Once);

            _mockRestClient.Verify(m => m.Execute(It.Is<RestRequest>(request => request.Parameters.Any(
                param => param.Type == ParameterType.RequestBody && param.ContentType == @"application/json"))), Times.Once);

            response.Should().NotBeNull();
            response.ErrorDataCollection.Should().BeNull();
            response.Status.Should().Equals(0);
        }
        
        [TestMethod]
        public void VerifyCard_WhenVerifyIsFailedAndHasOneError_ShouldFillResponseCorrectly()
        {
            var verifyCardRequest = VerifyCardHelper.ValidVerifyCardRequest();

            _mockRestClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(new RestResponse<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = VerifyCardHelper.InvalidVerifyCardResponseOneError(),
                Data = VerifyCardHelper.InvalidVerifyCardResponseOneError()
            });

            var response = _verifyCardService.VerifyCard(new MerchantAuthentication(), verifyCardRequest);

            response.Should().NotBeNull();
            response.ErrorDataCollection.Should().NotBeNull();
            response.ErrorDataCollection.Should().HaveCount(1);
            response.ErrorDataCollection.Should().ContainSingle(o => o.Code == 322 && o.Message == "Verify card is not enabled");
        }

        [TestMethod]
        public void VerifyCard_WhenVerifyIsFailedAndHasTwoError_ShouldFillResponseCorrectly()
        {
            var verifyCardRequest = VerifyCardHelper.ValidVerifyCardRequest();

            _mockRestClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(new RestResponse<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = VerifyCardHelper.InvalidVerifyCardResponseTwoErrors(),
                Data = VerifyCardHelper.InvalidVerifyCardResponseTwoErrors()
            });

            var response = _verifyCardService.VerifyCard(new MerchantAuthentication(), verifyCardRequest);

            response.Should().NotBeNull();
            response.ErrorDataCollection.Should().NotBeNull();
            response.ErrorDataCollection.Should().HaveCount(2);
            response.ErrorDataCollection.Should().ContainSingle(o => o.Code == 322 && o.Message == "Verify card is not enabled");
            response.ErrorDataCollection.Should().ContainSingle(o => o.Code == 1 && o.Message == "Unexpected end of file");
        }


        [TestMethod]
        public void VerifyCard_WhenHttpStatusCodeEqualsTo0_ReturnsMappedErrorResponse()
        {
            var restResponse = new RestResponse
            {
                StatusCode = 0,
                ErrorException = new Exception("Test exception"),
                ErrorMessage = "Undefined Error"
            };

            _mockRestClient.Setup(m => m.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var response = _verifyCardService.VerifyCard(MerchantAuthenticationHelper.CreateMerchantAuthentication(), VerifyCardHelper.ValidVerifyCardRequest());

            response.Should().NotBeNull();
            Assert.AreEqual(response.HttpStatus, restResponse.StatusCode);
            response.ErrorDataCollection.Should().NotBeNull();
            response.ErrorDataCollection[0].Code.Should().Be(-1);
            response.ErrorDataCollection[0].Message.Should().Be("ErrorMessage: Undefined Error | ErrorException: System.Exception: Test exception");
        }
    }
}
