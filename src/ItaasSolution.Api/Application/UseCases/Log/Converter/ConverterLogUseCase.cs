﻿using FluentValidation;
using ItaasSolution.Api.Application.Formatting.Log;
using ItaasSolution.Api.Application.Validations.Log;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Infraestructure.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Converter
{
    public class ConverterLogUseCase : IConverterLogUseCase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogListFormatter _logListFormatter;
        private readonly IFileGenerator _fileGenerator;
        
        public ConverterLogUseCase(IConfiguration configuration, ILogListFormatter logListFormatter, IFileGenerator fileGenerator)
        {
            _fileGenerator = fileGenerator;
            _configuration = configuration;
            _logListFormatter = logListFormatter;
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
            }
            else
            { // Is the url with an file of an log in that the content the file must be converted in format solicited.
                string contentFileLog = string.Empty;

                // If the URL with file the is not empty gets the string the file
                if (!string.IsNullOrWhiteSpace(request.UrlLog))
                    contentFileLog = await ReadStringFromUrlAsync(request.UrlLog);

                logs = new List<ItaasSolution.Api.Domain.Entities.Log>();
                
                // Formats the string of the log in an list of the log
                var logList = _logListFormatter.ListFormatter(contentFileLog);

                // Makes the validation of the log data
                ValidateLogData(logList);

                foreach (var logLine in logList)
                {
                    var parts = logLine.Split('|');
                    var methodAndPath = parts[3].Split(' ');

                    var log = new ItaasSolution.Api.Domain.Entities.Log
                    {
                        ResponseSize = long.Parse(parts[0]),
                        StatusCode = long.Parse(parts[1]),
                        CacheStatus = parts[2],
                        HtttpMethod = methodAndPath[0].Trim('"'),
                        UriPath = methodAndPath[1],
                        TimeTaken = decimal.Parse(parts[4])
                    };

                    logs.Add(log);
                }
            }

            string urlFileLogConverted = string.Empty;
            string contentTextLogConverted = string.Empty;
            
            if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.UrlFile)
                urlFileLogConverted = await GenerateUrlFileLogAsync(logs);
            else if (request.FormatMadeAvailableLogConverted == Communication.Enums.FormatMadeAvailableLogConverted.ContentText)
                contentTextLogConverted = GenerateContentTextLogAgora(logs);

            return new ResponseConverterLogJson()
            {
                UrlFileLogConverted = urlFileLogConverted,
                ContentTextLogConverted = contentTextLogConverted,
            };
        }

        // This method converts the datas to the format Agora
        private string GenerateContentTextLogAgora(List<ItaasSolution.Api.Domain.Entities.Log> logs)
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
            var contentTextLogConverted = GenerateContentTextLogAgora(logs);

            // saves a file
            var result = await _fileGenerator.FileGeneratorAsync(contentTextLogConverted);

            // generates the url with the file
            if (result.fileGenerated == true)
            {
                var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
                var requestPath = fileRepositories
                    .FirstOrDefault(repo => repo["Tag"] == "agora-logs")?["RequestPath"];

                return "https://localhost:44395" + requestPath + "/" + result.nameFile;
            }
            else
            {
                throw new ErrorOnValidationException(new List<string>() { ResourceErrorMessages.FILE_GENERATOR_ERROR });
            }
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
        private void ValidateLogData(string[] logList)
        {
            var logDataValidator = new LogDataValidator();
            var resultLogDataValidator = logDataValidator.Validate(logList);

            if (resultLogDataValidator.IsValid == false)
            {
                var errorMessagesLogDataValidator = resultLogDataValidator.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessagesLogDataValidator);
            }
        }

        // This method get the content the an file the text of an URL
        public async Task<string> ReadStringFromUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}
