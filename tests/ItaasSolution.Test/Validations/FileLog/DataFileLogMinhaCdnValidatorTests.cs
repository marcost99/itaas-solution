using FluentAssertions;
using ItaasSolution.Api.Application.Validations.FileLog;
using ItaasSolution.Api.Exception;
using Xunit;

namespace ItaasSolution.Test.Validations.FileLog
{
    public class DataFileLogMinhaCdnValidatorTests
    {
        [Fact]
        public void Success()
        {
            var validator = new DataFileLogMinhaCdnValidator(); //initializes the class of validation
            var logArray = new string[] { "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2", "101|200|MISS|\"POST /myImages HTTP/1.1\"|319.4", "199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9", "312|200|INVALIDATE|\"GET /robots.txt HTTP/1.1\"|245.1" };

            var result = validator.Validate(logArray); //validates the datas of request

            result.IsValid.Should().BeTrue(); //verifies the results of validations
        }

        [Fact]
        public void Error_Invalid_Format_Data_Log()
        {
            var validator = new DataFileLogMinhaCdnValidator(); //initializes the class of validation
            var logArray = new string[] { "This is not an string with an format valid to data log." };

            var result = validator.Validate(logArray); //validates the datas of request

            result.IsValid.Should().BeFalse(); //verifies the results of validations
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.LOG_AGORA_DATA_INVALID));
        }
    }
}
