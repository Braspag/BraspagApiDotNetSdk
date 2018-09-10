using BraspagApiDotNetSdk.Contracts;
using BraspagApiDotNetSdk.Contracts.Enum;

namespace BraspagApiDotNetSdk.Tests.Helpers
{
    public class VerifyCardHelper
    {
        public static VerifyCardRequest ValidVerifyCardRequest()
        {
            return new VerifyCardRequest
            {
                Card = new Card
                {
                    CardNumber = "1234567891234567",
                    Brand = BrandEnum.Visa,
                    ExpirationDate = "12/2024",
                    Holder = "AUDREY ALBERTSON",
                    SecurityCode = "659"
                },
                Provider = ProviderEnum.Simulado
            };
        }

        public static string ValidVerifyCardResponseWithTrueResponse()
        {
            return "{ \"Valid\": true }";
        }

        public static VerifyCardRequest InvalidVerifyCardRequest()
        {
            return new VerifyCardRequest
            {
                Card = new Card
                {
                    CardNumber = "1234567891234588",
                    Brand = BrandEnum.Visa,
                    ExpirationDate = "12/2024",
                    Holder = "AUDREY ALBERTSON",
                    SecurityCode = "659"
                },
                Provider = ProviderEnum.Simulado
            };
        }

        public static string ValidVerifyCardResponseWithFalseResponse()
        {
            return "{ \"Valid\": false }";
        }

        public static string InvalidVerifyCardResponseOneError()
        {
            return "[ { \"Code\": 322, \"Message\": \"Verify card is not enabled\" } ]";
        }
        public static string InvalidVerifyCardResponseTwoErrors()
        {
            return "[ { \"Code\": 322, \"Message\": \"Verify card is not enabled\" }, { \"Code\": 1, \"Message\": \"Unexpected end of file\" } ]";
        }
    }
}
