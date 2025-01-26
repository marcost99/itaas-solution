using ItaasSolution.Api.Exception;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Communication.Responses;
using Microsoft.AspNetCore.Http;

namespace ItaasSolution.Api.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        // Implements function of interface
        public void OnException(ExceptionContext context)
        {
            // If error of exception configured
            if (context.Exception is ItaasSolutionException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknownError(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            var itaasSolutionException = context.Exception as ItaasSolutionException;
            var errorResponse = new ResponseErrorJson(itaasSolutionException.GetErrors());

            context.HttpContext.Response.StatusCode = itaasSolutionException.StatusCode;
            context.Result = new ObjectResult(errorResponse);
        }

        private void ThrowUnknownError(ExceptionContext context)
        {
            var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
