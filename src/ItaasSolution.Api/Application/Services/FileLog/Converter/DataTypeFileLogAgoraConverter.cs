using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Infraestructure.Services.File.Generator;
using ItaasSolution.Api.Infraestructure.Services.FileLog.Info;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.Services.FileLog.Converter
{
    public class DataTypeFileLogAgoraConverter : IDataTypeFileLogConverter
    {
        private readonly IGeneratorFile _fileGenerator;
        private readonly IInfoFileLog _infoFileLog;

        public DataTypeFileLogAgoraConverter(IGeneratorFile fileGenerator, IInfoFileLog infoFileLog)
        {
            _fileGenerator = fileGenerator;
            _infoFileLog = infoFileLog;
        }

        // This method converts the datas to the format Agora
        public string ConverterListObjectToStringFileLog(List<Domain.Entities.Log> logs)
        {
            var contentTextLog = $"#Version: 1.0\n" +
                   $"#Date: {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")}\n" +
                   $"#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n";

            foreach (var log in logs)
                contentTextLog += $"\"MINHA CDN\" {log.HtttpMethod} {log.StatusCode} {log.UriPath} {(uint)log.TimeTaken} {log.ResponseSize} {log.CacheStatus}\n";

            return contentTextLog;
        }

        // This method converts the datas to the format Agora and saves an file
        public async Task<string> ConverterListObjectToUrlFileLogAsync(List<Domain.Entities.Log> logs, long idFileLog)
        {
            // converts the datas to the format Agora
            var contentTextLogConverted = ConverterListObjectToStringFileLog(logs);

            // saves an file
            var tag = "agora-logs";
            var physicalPath = _infoFileLog.GetPhysicalPath(tag);
            var fileName = _infoFileLog.GetFileNameFull(idFileLog);

            var result = await _fileGenerator.FileGeneratorAsync(contentTextLogConverted, physicalPath, fileName);

            // generates the url with the file
            if (result == true)
            {
                var urlHost = _infoFileLog.GetUrlHost();
                var urlPath = _infoFileLog.GetUrlPath(tag);

                return urlHost + urlPath + "/" + fileName;
            }
            else
            {
                throw new ErrorOnValidationException(new List<string>() { ResourceErrorMessages.FILE_GENERATOR_ERROR });
            }
        }

        // This method get the content the an file the text of an URL
        public async Task<string> ConverterFileUrlToStringFileLogAsync(string url)
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
