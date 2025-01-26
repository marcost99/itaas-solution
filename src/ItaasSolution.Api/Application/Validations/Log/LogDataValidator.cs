using FluentValidation;
using ItaasSolution.Api.Exception;
using System.Text.RegularExpressions;

namespace ItaasSolution.Api.Application.Validations.Log
{
    public class LogDataValidator : AbstractValidator<string>
    {
        public LogDataValidator()
        {
            RuleFor(logData => logData)
                .Must(logData =>
                {
                    var lines = logData.Split('\n');
                    string pattern = @"^\d+\|\d+\|(HIT|MISS|INVALIDATE)\|""(GET|POST) [^""]+ HTTP/1\.1""\|\d+(\.\d+)?$";
                    foreach (var line in lines)
                    {
                        if (!Regex.IsMatch(line, pattern))
                        {
                            return false;
                        }
                    }
                    return true;
                })
                .WithMessage(ResourceErrorMessages.LOG_DATA_INVALID);
        }
    }
}
