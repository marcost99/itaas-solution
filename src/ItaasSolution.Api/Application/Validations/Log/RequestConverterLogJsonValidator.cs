using FluentValidation;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Exception;
using System;

namespace ItaasSolution.Api.Application.Validations.Log
{
    public class RequestConverterLogJsonValidator : AbstractValidator<RequestConverterLogJson>
    {
        public RequestConverterLogJsonValidator()
        {
            RuleFor(converterLog => converterLog.FormatMadeAvailableLogConverted).IsInEnum().WithMessage(ResourceErrorMessages.FORMAT_LOG_INVALID);
            RuleFor(converterLog => converterLog.IdLog).GreaterThan(0).WithMessage(ResourceErrorMessages.ID_LOG_GREATER_THAN_ZERO).When(converterLog => string.IsNullOrWhiteSpace(converterLog.UrlLog));
            RuleFor(converterLog => converterLog.UrlLog)
                .NotEmpty().WithMessage(ResourceErrorMessages.URL_LOG_EMPTY)
                .Must(BeAValidUrl).WithMessage(ResourceErrorMessages.URL_LOG_INVALID)
                .When(converterLog => converterLog.IdLog <= 0);
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
