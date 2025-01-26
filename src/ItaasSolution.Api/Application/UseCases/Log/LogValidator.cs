using FluentValidation;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Exception;

namespace ItaasSolution.Api.Application.UseCases.Log
{
    public class LogValidator : AbstractValidator<RequestConverterLogJson>
    {
        public LogValidator() 
        {
            RuleFor(converterLog => converterLog.FormatMadeAvailableLogConverted).IsInEnum().WithMessage(ResourceErrorMessages.FORMAT_LOG_INVALID);
            RuleFor(converterLog => converterLog.IdLog).GreaterThan(0).WithMessage(ResourceErrorMessages.ID_LOG_GREATER_THAN_ZERO).When(converterLog => string.IsNullOrWhiteSpace(converterLog.UrlLog));
            RuleFor(converterLog => converterLog.UrlLog).NotEmpty().WithMessage(ResourceErrorMessages.URL_LOG_EMPTY).When(converterLog => converterLog.IdLog <= 0);
        }
    }
}
