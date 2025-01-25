using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Converter
{
    public class ConverterLogUseCase : IConverterLogUseCase
    {
        public async Task<ResponseConverterLogJson> Execute(RequestConverterLogJson request)
        {
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
            
            if (request.Format == Communication.Enums.FormatContentLogConverted.UrlFile)
                urlFileLogConverted = await GenerateUrlFileLog(logs);
            else if (request.Format == Communication.Enums.FormatContentLogConverted.ContentText)
                contentTextLogConverted = GenerateContentTextLog(logs);

            return new ResponseConverterLogJson()
            {
                UrlFileLogConverted = urlFileLogConverted,
                ContentTextLogConverted = contentTextLogConverted,
            };
        }

        // This method converts the datas to the format Agora
        public string GenerateContentTextLog(List<ItaasSolution.Api.Domain.Entities.Log> logs)
        {
            var contentTextLog = $"#Version: 1.0\n" +
                   $"#Date: {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")}\n" +
                   $"#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n";
            
            foreach(var log in logs)
                contentTextLog += $"\"MINHA CDN\" {log.HtttpMethod} {log.StatusCode} {log.UriPath} {(uint)log.TimeTaken} {log.ResponseSize} {log.CacheStatus}\n";

            return contentTextLog;
        }

        // This method converts the datas to the format Agora and saves an file
        public async Task<string> GenerateUrlFileLog(List<ItaasSolution.Api.Domain.Entities.Log> logs)
        {
            var contentTextLogConverted = GenerateContentTextLog(logs);

            var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var repositoryPath = Path.Combine(solutionDirectory, "repository\\logs\\");

            var now = DateTime.UtcNow;
            var nameFile = @"input_" + $"{now.Year}{now.Month:D2}{now.Day:D2}{now.Hour:D2}{now.Minute:D2}{now.Second:D2}{now.Millisecond:D3}.txt";
            var file = repositoryPath + nameFile;

            await WriteToFileAsync(contentTextLogConverted, file);

            if (File.Exists(file))
                return "https://localhost:44395/logs/" + nameFile;
            else
                throw new Exception("Error an saved file of the log.");
        }

        // This method save an file
        public async Task WriteToFileAsync(string conteudo, string caminhoArquivo)
        {
            using (StreamWriter writer = new StreamWriter(caminhoArquivo))
            {
                await writer.WriteAsync(conteudo);
            }
        }
    }
}
