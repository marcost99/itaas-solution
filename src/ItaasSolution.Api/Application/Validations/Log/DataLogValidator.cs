using FluentValidation;
using ItaasSolution.Api.Exception;
using System.Text.RegularExpressions;

namespace ItaasSolution.Api.Application.Validations.Log
{
    public class DataLogValidator : AbstractValidator<string[]>
    {
        public DataLogValidator()
        {
            RuleFor(logData => logData)
                .Must(logData =>
                {
                    string pattern = @"^\d+\|\d+\|(HIT|MISS|INVALIDATE)\|""(GET|POST) [^""]+ HTTP\/1\.1""\|\d+\.\d+$";
                    foreach (var line in logData)
                    {
                        if (!Regex.IsMatch(line.Trim(), pattern))
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
