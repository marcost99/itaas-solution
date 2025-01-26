using FluentAssertions;
using ItaasSolution.Api.Application.Validations.Log;
using ItaasSolution.Api.Communication.Enums;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Exception;
using Xunit;

namespace ItaasSolution.Test.Validations
{
    public class ConverterLogValidatorTests
    {
        [Fact]
        public void Success()
        {
            var validator = new RequestConverterLogJsonValidator(); //initializes the class of validation
            var request = new RequestConverterLogJson() 
            {
                FormatMadeAvailableLogConverted = FormatMadeAvailableLogConverted.UrlFile,
                UrlLog = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"
            }; //simulates the request

            var result = validator.Validate(request); //validates the datas of request

            result.IsValid.Should().BeTrue(); //verifies the results of validations
        }

        [Fact]
        public void Errors_Format_IdLog_Url()
        {
            var validator = new RequestConverterLogJsonValidator(); //initializes the class of validation
            var request = new RequestConverterLogJson()
            {
                FormatMadeAvailableLogConverted = (FormatMadeAvailableLogConverted)2,
                IdLog = 0,
                UrlLog = string.Empty
            }; //simulates the request

            var result = validator.Validate(request); //validates the datas of request

            result.IsValid.Should().BeFalse(); //verifies the results of validations
            result.Errors.Should()
                .Contain
                (
                    e => e.ErrorMessage.Equals(ResourceErrorMessages.FORMAT_LOG_INVALID) ||
                    e.ErrorMessage.Equals(ResourceErrorMessages.ID_LOG_GREATER_THAN_ZERO) ||
                    e.ErrorMessage.Equals(ResourceErrorMessages.URL_LOG_EMPTY) ||
                    e.ErrorMessage.Equals(ResourceErrorMessages.URL_LOG_INVALID)
                )
                .And.HaveCount(4);
        }
    }
}
