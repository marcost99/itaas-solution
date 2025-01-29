using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ItaasSolution.Api.Infraestructure.Services.FileLog.Info
{
    public class InfoFileLog : IInfoFileLog
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public InfoFileLog(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string GetUrlHost()
        {
            var httpContext = _httpContextAccessor.HttpContext.Request;
            return $"{httpContext.Scheme}://{httpContext.Host}";
        }

        public string GetUrlPath(string tag)
        {
            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var urlPath = fileRepositories
                    .FirstOrDefault(repo => repo["Tag"] == tag)?["RequestPath"];
            return urlPath;
        }

        public string GetFileNameBase()
        {
            var fileNameBase = _configuration.GetSection("Settings:FileName").Get<string>();
            return fileNameBase;
        }

        public string GetFileNameFull(long idFileLog)
        {
            var fileNameBase = GetFileNameBase();
            var fileNameFull = string.Format("{0}-{1}.txt", fileNameBase, idFileLog);
            return fileNameFull;
        }

        public string GetRegexFileName()
        {
            var fileNameBase = GetFileNameBase();
            var regex = fileNameBase + @"-(\d+)\.txt";
            return regex;
        }

        public string GetRegexFileNameId()
        {
            return @"\d+";
        }

        public long GetFileId(string path)
        {
            long idFileLog = 0;
            var regex = GetRegexFileName();
            var match = Regex.Match(path, regex);
            if (match.Success)
                idFileLog = int.Parse(match.Groups[1].Value);
            return idFileLog;
        }

        public string GetPhysicalPath(string tag)
        {
            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var physicalPath = fileRepositories
                .FirstOrDefault(repo => repo["Tag"] == tag)?["PhysicalPath"];
            return physicalPath;
        }
    }
}
