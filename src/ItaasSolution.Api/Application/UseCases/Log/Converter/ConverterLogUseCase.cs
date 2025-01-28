using ItaasSolution.Api.Application.Conversions.Log;
using ItaasSolution.Api.Application.Formatting.Log;
using ItaasSolution.Api.Application.Services;
using ItaasSolution.Api.Application.Validations.Log;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Domain.Repositories.Logs;
using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Converter
{
    public class ConverterLogUseCase : IConverterLogUseCase
    {
        private readonly ILogsReadOnlyRepository _repository;
        private readonly IDataTypeLogConverter _dataTypeLogConverter;
        private readonly IFormatContentLogConverter _formatContentAgoraLogConverter;
        private readonly IInfoFileLog _infoFileLog;

        public ConverterLogUseCase(ILogsReadOnlyRepository repository, IDataTypeLogConverter dataTypeLogConverter, IFormatContentLogConverter formatContentAgoraLogConverter, IInfoFileLog infoFileLog)
        {
            _repository = repository;
            _dataTypeLogConverter = dataTypeLogConverter;
            _formatContentAgoraLogConverter = formatContentAgoraLogConverter;
            _infoFileLog = infoFileLog;
        }
        
        public async Task<ResponseConverterLogJson> ExecuteAsync(RequestConverterLogJson request)
        {
            // Makes the validations
            await ValidateRequest(request);

            List<ItaasSolution.Api.Domain.Entities.Log> logs;

            if (request.IdLog > 0)
            { // Is the id of an log of the database in that the data must be gotten and converted in format solicited.
                
                var log = await _repository.GetByIdAsync(request.IdLog);

                if (log == null)
                {
                    throw new NotFoundException(ResourceErrorMessages.LOG_NOT_FOUND);
                }

                logs = new List<ItaasSolution.Api.Domain.Entities.Log>()
                {
                    new ItaasSolution.Api.Domain.Entities.Log
                    (
                        log.HtttpMethod,
                        log.StatusCode,
                        log.UriPath,
                        log.TimeTaken,
                        log.ResponseSize,
                        log.CacheStatus,
                        DateTime.UtcNow
                    )
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
            {
                // Sets the id of the file log
                long idFileLog = 0;
                if (request.IdLog > 0)
                    idFileLog = _infoFileLog.NewFileId();
                else
                    idFileLog = _infoFileLog.FileId(request.UrlLog);

                urlFileLogConverted = await _formatContentAgoraLogConverter.ConverterListObjectToUrlFileLogAsync(logs, idFileLog);
            }
            else if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.ContentText)
            {
                contentTextLogConverted = _formatContentAgoraLogConverter.ConverterListObjectToStringLog(logs);
            }

            return new ResponseConverterLogJson()
            {
                UrlFileLogConverted = urlFileLogConverted,
                ContentTextLogConverted = contentTextLogConverted,
            };
        }

        // This method makes the validation of the request
        private async Task ValidateRequest(RequestConverterLogJson request)
        {
            var requestValidator = new RequestConverterLogJsonValidator(_infoFileLog);
            var resultRequestValidator = await requestValidator.ValidateAsync(request);

            if (resultRequestValidator.IsValid == false)
            {
                var errorMessagesValidatorRequest = resultRequestValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesValidatorRequest);
            }
        }

        // This method makes the validation of the log data
        private void ValidateDataLog(string[] logArray)
        {
            var dataLogMinhaCdnValidator = new DataLogMinhaCdnValidator();
            var resultLogDataValidator = dataLogMinhaCdnValidator.Validate(logArray);

            if (resultLogDataValidator.IsValid == false)
            {
                var errorMessagesLogDataValidator = resultLogDataValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesLogDataValidator);
            }
        }
    }
}
