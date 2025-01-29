using ItaasSolution.Api.Application.Services.FileLog.Converter;
using ItaasSolution.Api.Application.Services.Log.Converter;
using ItaasSolution.Api.Application.Validations.FileLog;
using ItaasSolution.Api.Communication.Requests.FileLog;
using ItaasSolution.Api.Communication.Responses.FileLog;
using ItaasSolution.Api.Domain.Repositories.FileLogs;
using ItaasSolution.Api.Domain.Repositories.Logs;
using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Infraestructure.Services.FileLog.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.FileLog.Converter
{
    public class ConverterFileLogUseCase : IConverterFileLogUseCase
    {
        private readonly ILogsReadOnlyRepository _repository;
        private readonly IDataTypeLogConverter _dataTypeLogConverter;
        private readonly IDataTypeFileLogConverter _dataTypeFileLogConverter;
        private readonly IInfoFileLog _infoFileLog;
        private readonly IFileLogsReadOnlyRepository _fileLogsReadOnlyRepository;

        public ConverterFileLogUseCase(ILogsReadOnlyRepository repository, IDataTypeLogConverter dataTypeLogConverter, IDataTypeFileLogConverter dataTypeFileLogConverter, IInfoFileLog infoFileLog, IFileLogsReadOnlyRepository fileLogsReadOnlyRepository)
        {
            _repository = repository;
            _dataTypeLogConverter = dataTypeLogConverter;
            _dataTypeFileLogConverter = dataTypeFileLogConverter;
            _infoFileLog = infoFileLog;
            _fileLogsReadOnlyRepository = fileLogsReadOnlyRepository;
        }

        public async Task<ResponseConverterFileLogJson> ExecuteAsync(RequestConverterFileLogJson request)
        {
            // Makes the validations
            await ValidateRequest(request);

            List<Domain.Entities.Log> logs;

            if (request.IdLog > 0)
            { // Is the id of an log of the database in that the data must be gotten and converted in format solicited.

                var log = await _repository.GetByIdAsync(request.IdLog);

                if (log == null)
                {
                    throw new NotFoundException(ResourceErrorMessages.LOG_NOT_FOUND);
                }

                logs = new List<Domain.Entities.Log>()
                {
                    new Domain.Entities.Log
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
                    contentFileLog = await _dataTypeFileLogConverter.ConverterFileUrlToStringFileLogAsync(request.UrlLog);

                // Formats the string of the log in an list of the log
                var logArray = _dataTypeLogConverter.ConverterStringToArrayLog(contentFileLog);

                // Makes the validation of the file log data
                ValidateDataFileLog(logArray);

                // Converts array of the logs in list of the object
                logs = _dataTypeLogConverter.ConverterArrayToListObjectLog(logArray);
            }

            string urlFileLogConverted = string.Empty;
            string contentTextFileLogConverted = string.Empty;

            if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.UrlFile)
            {
                // Sets the id of the file log
                long idFileLog = 0;
                if (request.IdLog > 0)
                    idFileLog = _fileLogsReadOnlyRepository.GetNewFileId();
                else
                    idFileLog = _infoFileLog.GetFileId(request.UrlLog);

                urlFileLogConverted = await _dataTypeFileLogConverter.ConverterListObjectToUrlFileLogAsync(logs, idFileLog);
            }
            else if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.ContentText)
            {
                contentTextFileLogConverted = _dataTypeFileLogConverter.ConverterListObjectToStringFileLog(logs);
            }

            return new ResponseConverterFileLogJson()
            {
                UrlFileLogConverted = urlFileLogConverted,
                ContentTextFileLogConverted = contentTextFileLogConverted,
            };
        }

        // This method makes the validation of the request
        private async Task ValidateRequest(RequestConverterFileLogJson request)
        {
            var requestValidator = new RequestConverterFileLogJsonValidator(_infoFileLog);
            var resultRequestValidator = await requestValidator.ValidateAsync(request);

            if (resultRequestValidator.IsValid == false)
            {
                var errorMessagesValidatorRequest = resultRequestValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesValidatorRequest);
            }
        }

        // This method makes the validation of the log data
        private void ValidateDataFileLog(string[] logArray)
        {
            var dataFileLogMinhaCdnValidator = new DataFileLogMinhaCdnValidator();
            var resultLogDataValidator = dataFileLogMinhaCdnValidator.Validate(logArray);

            if (resultLogDataValidator.IsValid == false)
            {
                var errorMessagesLogDataValidator = resultLogDataValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesLogDataValidator);
            }
        }
    }
}
