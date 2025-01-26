using ItaasSolution.Api.Application.Conversions.Log;
using ItaasSolution.Api.Application.Formatting.Log;
using ItaasSolution.Api.Application.Validations.Log;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Exception.ExceptionsBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Converter
{
    public class ConverterLogUseCase : IConverterLogUseCase
    {
        private readonly IDataTypeLogConverter _dataTypeLogConverter;
        private readonly IFormatContentAgoraLogConverter _formatContentAgoraLogConverter;

        public ConverterLogUseCase(IDataTypeLogConverter logConverter, IFormatContentAgoraLogConverter formatContentAgoraLogConverter)
        {
            _dataTypeLogConverter = logConverter;
            _formatContentAgoraLogConverter = formatContentAgoraLogConverter;
        }
        
        public async Task<ResponseConverterLogJson> ExecuteAsync(RequestConverterLogJson request)
        {
            // Makes the validations
            ValidateRequest(request);

            List<ItaasSolution.Api.Domain.Entities.Log> logs;

            if (request.IdLog > 0)
            { // Is the id of an log of the database in that the data must be gotten and converted in format solicited.
                logs = new List<ItaasSolution.Api.Domain.Entities.Log>()
                {
                    new ItaasSolution.Api.Domain.Entities.Log()
                    {
                        Id = request.IdLog,
                        HtttpMethod = "GET",
                        StatusCode = 200,
                        UriPath = "/robots.txt",
                        TimeTaken = (decimal)100.2,
                        ResponseSize = 312,
                        CacheStatus = "HIT",
                    }
                };
            }
            else
            { // Is the url with an file of an log in that the content the file must be converted in format solicited.
                string contentFileLog = string.Empty;

                // If the URL with file the is not empty gets the string the file
                if (!string.IsNullOrWhiteSpace(request.UrlLog))
                    contentFileLog = await _dataTypeLogConverter.ConverterFileUrlToFileStringLogAsync(request.UrlLog);
                
                // Formats the string of the log in an list of the log
                var logArray = _dataTypeLogConverter.ConverterStringToArrayLog(contentFileLog);

                // Makes the validation of the log data
                ValidateDataLog(logArray);

                // Converts array of the logs in list of the object
                logs = _dataTypeLogConverter.ConverterArrayToListObjectLog(logArray);
            }

            string urlFileLogConverted = string.Empty;
            string contentTextLogConverted = string.Empty;
            
            if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.UrlFile)
                urlFileLogConverted = await _formatContentAgoraLogConverter.ConverterListObjectToUrlFileLogAgoraAsync(logs);
            else if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.ContentText)
                contentTextLogConverted = _formatContentAgoraLogConverter.ConverterListObjectToStringLogAgora(logs);

            return new ResponseConverterLogJson()
            {
                UrlFileLogConverted = urlFileLogConverted,
                ContentTextLogConverted = contentTextLogConverted,
            };
        }

        // This method makes the validation of the request
        private void ValidateRequest(RequestConverterLogJson request)
        {
            var requestValidator = new RequestConverterLogJsonValidator();
            var resultRequestValidator = requestValidator.Validate(request);

            if (resultRequestValidator.IsValid == false)
            {
                var errorMessagesValidatorRequest = resultRequestValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesValidatorRequest);
            }
        }

        // This method makes the validation of the log data
        private void ValidateDataLog(string[] logArray)
        {
            var dataLogAgoraValidator = new DataLogAgoraValidator();
            var resultLogDataValidator = dataLogAgoraValidator.Validate(logArray);

            if (resultLogDataValidator.IsValid == false)
            {
                var errorMessagesLogDataValidator = resultLogDataValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesLogDataValidator);
            }
        }
    }
}
