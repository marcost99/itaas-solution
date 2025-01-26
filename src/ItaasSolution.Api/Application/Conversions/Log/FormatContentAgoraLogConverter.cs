using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Infraestructure.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Conversions.Log
{
    public class FormatContentAgoraLogConverter : IFormatContentLogConverter
    {
        private readonly IConfiguration _configuration;
        private readonly IFileGenerator _fileGenerator;

        public FormatContentAgoraLogConverter(IConfiguration configuration, IFileGenerator fileGenerator)
        {
            _configuration = configuration;
            _fileGenerator = fileGenerator;
        }

        // This method converts the datas to the format Agora
        public string ConverterListObjectToStringLog(List<ItaasSolution.Api.Domain.Entities.Log> logs)
        {
            var contentTextLog = $"#Version: 1.0\n" +
                   $"#Date: {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")}\n" +
                   $"#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n";

            foreach (var log in logs)
                contentTextLog += $"\"MINHA CDN\" {log.HtttpMethod} {log.StatusCode} {log.UriPath} {(uint)log.TimeTaken} {log.ResponseSize} {log.CacheStatus}\n";

            return contentTextLog;
        }

        // This method converts the datas to the format Agora and saves an file
        public async Task<string> ConverterListObjectToUrlFileLogAsync(List<ItaasSolution.Api.Domain.Entities.Log> logs, long idFileLog)
        {
            // converts the datas to the format Agora
            var contentTextLogConverted = ConverterListObjectToStringLog(logs);

            // saves an file
            var tag = "agora-logs";
            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var physicalPath = fileRepositories
                .FirstOrDefault(repo => repo["Tag"] == tag)?["PhysicalPath"];
            var fileName = string.Format("input-{0}.txt", idFileLog);
            
            var result = await _fileGenerator.FileGeneratorAsync(contentTextLogConverted, physicalPath, fileName);

            // generates the url with the file
            if (result == true)
            {
                var requestPath = fileRepositories
                    .FirstOrDefault(repo => repo["Tag"] == tag)?["RequestPath"];

                return "https://localhost:44395" + requestPath + "/" + fileName;
            }
            else
            {
                throw new ErrorOnValidationException(new List<string>() { ResourceErrorMessages.FILE_GENERATOR_ERROR });
            }
        }
    }
}
