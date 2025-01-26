using FluentValidation;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Exception;

namespace ItaasSolution.Api.Application.Validations.Log
{
    public class RequestLogJsonValidator : AbstractValidator<RequestLogJson>
    {
        public RequestLogJsonValidator()
        {
            RuleFor(log => log.HtttpMethod)
                .NotEmpty().WithMessage(ResourceErrorMessages.HTTP_METHOD_NOT_EMPTY)
                .MaximumLength(20).WithMessage(ResourceErrorMessages.HTTP_METHOD_MAX_SIZE);
            RuleFor(log => log.StatusCode).GreaterThan(0).WithMessage(ResourceErrorMessages.STATUS_CODE_GREATER_THAN_ZERO);
            RuleFor(log => log.UriPath)
                .NotEmpty().WithMessage(ResourceErrorMessages.URI_PATH_NOT_EMPTY)
                .MaximumLength(50).WithMessage(ResourceErrorMessages.URI_PATH_MAX_SIZE);
            RuleFor(log => log.TimeTaken).GreaterThan(0).WithMessage(ResourceErrorMessages.TIME_TAKEN_GREATER_THAN_ZERO);
            RuleFor(log => log.ResponseSize).GreaterThan(0).WithMessage(ResourceErrorMessages.RESPONSE_SIZE_GREATER_THAN_ZERO);
            RuleFor(log => log.CacheStatus)
                .NotEmpty().WithMessage(ResourceErrorMessages.CACHE_STATUS_NOT_EMPTY)
                .MaximumLength(20).WithMessage(ResourceErrorMessages.CACHE_STATUS_MAX_SIZE);
        }
    }
}
