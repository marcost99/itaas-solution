using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Infraestructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Converter
{
    public class ConverterLogUseCase : IConverterLogUseCase
    {
        private readonly IFileGenerator _fileGenerator;
        
        public ConverterLogUseCase(IFileGenerator fileGenerator)
        {
            _fileGenerator = fileGenerator;
        }
        
        public async Task<ResponseConverterLogJson> ExecuteAsync(RequestConverterLogJson request)
        {
            Validate(request);

            var logs = new List<ItaasSolution.Api.Domain.Entities.Log>()
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
                },
                new ItaasSolution.Api.Domain.Entities.Log()
                {
                    Id = request.IdLog,
                    HtttpMethod = "POST",
                    StatusCode = 200,
                    UriPath = "/myImages",
                    TimeTaken = (decimal)319.4,
                    ResponseSize = 101,
                    CacheStatus = "MISS",
                }
            };

            string urlFileLogConverted = string.Empty;
            string contentTextLogConverted = string.Empty;
            
            if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.UrlFile)
                urlFileLogConverted = await GenerateUrlFileLogAsync(logs);
            else if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.ContentText)
                contentTextLogConverted = GenerateContentTextLog(logs);

            return new ResponseConverterLogJson()
            {
                UrlFileLogConverted = urlFileLogConverted,
                ContentTextLogConverted = contentTextLogConverted,
            };
        }

        // This method converts the datas to the format Agora
        private string GenerateContentTextLog(List<ItaasSolution.Api.Domain.Entities.Log> logs)
        {
            var contentTextLog = $"#Version: 1.0\n" +
                   $"#Date: {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")}\n" +
                   $"#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n";
            
            foreach(var log in logs)
                contentTextLog += $"\"MINHA CDN\" {log.HtttpMethod} {log.StatusCode} {log.UriPath} {(uint)log.TimeTaken} {log.ResponseSize} {log.CacheStatus}\n";

            return contentTextLog;
        }

        // This method converts the datas to the format Agora and saves an file
        private async Task<string> GenerateUrlFileLogAsync(List<ItaasSolution.Api.Domain.Entities.Log> logs)
        {
            // converts the datas to the format Agora
            var contentTextLogConverted = GenerateContentTextLog(logs);

            // saves a file
            var result = await _fileGenerator.FileGeneratorAsync(contentTextLogConverted, "input");

            // generates the url with the file
            if (result.fileGenerated == true)
                return "https://localhost:44395/logs/" + result.nameFile;
            else
                throw new ErrorOnValidationException(new List<string>() { ResourceErrorMessages.FILE_GENERATOR_ERROR });
        }

        // This method makes the validation of the request
        private void Validate(RequestConverterLogJson request)
        {
            var validator = new LogValidator();
            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
