using FluentValidation;
using ItaasSolution.Api.Application.Services.FileLog.Info;
using ItaasSolution.Api.Communication.Requests.FileLog;
using ItaasSolution.Api.Exception;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Validations.FileLog
{
    public class RequestConverterFileLogJsonValidator : AbstractValidator<RequestConverterFileLogJson>
    {
        private readonly IInfoFileLog _infoFileLog;
        private static readonly HttpClient _httpClient = new HttpClient();

        public RequestConverterFileLogJsonValidator(IInfoFileLog infoFileLog)
        {
            _infoFileLog = infoFileLog;

            RuleFor(converterLog => converterLog.FormatMadeAvailableLogConverted).IsInEnum().WithMessage(ResourceErrorMessages.FORMAT_LOG_INVALID);
            RuleFor(converterLog => converterLog.IdLog).GreaterThan(0).WithMessage(ResourceErrorMessages.ID_LOG_GREATER_THAN_ZERO).When(converterLog => string.IsNullOrWhiteSpace(converterLog.UrlLog));
            RuleFor(converterLog => converterLog.UrlLog)
                .NotEmpty().WithMessage(ResourceErrorMessages.URL_LOG_EMPTY)
                .Must(BeAValidUrl).WithMessage(ResourceErrorMessages.URL_LOG_INVALID)
                .MustAsync((url, cancellationToken) => UrlExistsAsync(url, cancellationToken)).WithMessage(ResourceErrorMessages.URL_LOG_NOT_EXISTS)
                .Must(BeFileId).WithMessage(string.Format(ResourceErrorMessages.ID_FILE_LOG_INVALID, _infoFileLog.FileNameBase()))
                .When(converterLog => converterLog.IdLog <= 0);

        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private bool BeFileId(string url)
        {
            var idFileLog = _infoFileLog.FileId(url);
            return idFileLog > 0;
        }

        public static async Task<bool> UrlExistsAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
