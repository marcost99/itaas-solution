using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ItaasSolution.Api.Application.Services.FileLog.Info
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

        public string UrlHost()
        {
            var httpContext = _httpContextAccessor.HttpContext.Request;
            return $"{httpContext.Scheme}://{httpContext.Host}";
        }

        public string UrlPath(string tag)
        {
            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var urlPath = fileRepositories
                    .FirstOrDefault(repo => repo["Tag"] == tag)?["RequestPath"];
            return urlPath;
        }

        public string FileNameBase()
        {
            var fileNameBase = _configuration.GetSection("Settings:FileName").Get<string>();
            return fileNameBase;
        }

        public string FileNameFull(long idFileLog)
        {
            var fileNameBase = FileNameBase();
            var fileNameFull = string.Format("{0}-{1}.txt", fileNameBase, idFileLog);
            return fileNameFull;
        }

        public long FileId(string path)
        {
            long idFileLog = 0;
            var fileNameBase = FileNameBase();
            var regex = fileNameBase + @"-(\d+)\.txt";
            var match = Regex.Match(path, regex);
            if (match.Success)
                idFileLog = int.Parse(match.Groups[1].Value);
            return idFileLog;
        }

        public long NewFileId()
        {
            var tag = "agora-logs";
            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var physicalPath = fileRepositories
                .FirstOrDefault(repo => repo["Tag"] == tag)?["PhysicalPath"];
            var fullPath = Path.GetFullPath(physicalPath);

            var fileNameBase = FileNameBase();
            var regex = fileNameBase + @"-(\d+)\.txt";

            var files = Directory.GetFiles(fullPath)
                             .Where(file => Regex.IsMatch(Path.GetFileName(file), regex))
                             .OrderBy(file => int.Parse(Regex.Match(Path.GetFileName(file), @"\d+").Value))
                             .ToList();

            var fileId = FileId(files[files.Count - 1]) + 1;

            return fileId;
        }

        public string PhysicalPath(string tag)
        {
            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var physicalPath = fileRepositories
                .FirstOrDefault(repo => repo["Tag"] == tag)?["PhysicalPath"];
            return physicalPath;
        }
    }
}
