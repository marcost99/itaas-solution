using FluentAssertions;
using ItaasSolution.Api.Application.Validations.FileLog;
using ItaasSolution.Api.Communication.Enums;
using ItaasSolution.Api.Communication.Requests.FileLog;
using ItaasSolution.Api.Infraestructure.Services.FileLog.Info;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace ItaasSolution.Test.Validations.FileLog
{
    public class ConverterFileLogValidatorTests : IClassFixture<DependencyInjectionFixture>
    {
        private readonly IInfoFileLog _infoFileLog;

        public ConverterFileLogValidatorTests(DependencyInjectionFixture fixture)
        {
            _infoFileLog = fixture.ServiceProvider.GetRequiredService<IInfoFileLog>();
        }

        [Fact]
        public async Task Success()
        {
            var validator = new RequestConverterFileLogJsonValidator(_infoFileLog); //initializes the class of validation
            var request = new RequestConverterFileLogJson()
            {
                FormatMadeAvailableLogConverted = FormatMadeAvailableLogConverted.UrlFile,
                UrlLog = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt"
            }; //simulates the request

            var result = await validator.ValidateAsync(request); //validates the datas of request

            result.IsValid.Should().BeTrue(); //verifies the results of validations
        }
    }
}
