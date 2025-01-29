using ItaasSolution.Api.Domain.Entities;
using ItaasSolution.Api.Domain.Repositories.FileLogs;
using ItaasSolution.Api.Infraestructure.Services.File.HandlerIO;
using ItaasSolution.Api.Infraestructure.Services.FileLog.Info;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.DataAccess.Repositories
{
    public class FileLogsRepository : IFileLogsReadOnlyRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IInfoFileLog _infoFileLog;
        private readonly IHandlerIOFile _handlerIOFile;

        public FileLogsRepository(IConfiguration configuration, IInfoFileLog infoFileLog, IHandlerIOFile handlerIOFile)
        {
            _configuration = configuration;
            _infoFileLog = infoFileLog;
            _handlerIOFile = handlerIOFile;
        }

        private List<string> GetFileLogs(string tag)
        {
            var physicalPath = _infoFileLog.GetPhysicalPath(tag);

            var fullPath = Path.GetFullPath(physicalPath);

            var regex = _infoFileLog.GetRegexFileName();

            var fileLogs = Directory.GetFiles(fullPath)
                             .Where(file => Regex.IsMatch(Path.GetFileName(file), regex))
                             .OrderBy(file => int.Parse(Regex.Match(Path.GetFileName(file), _infoFileLog.GetRegexFileNameId()).Value))
                             .ToList();

            return fileLogs;
        }

        public async Task<List<FileLog>> GetAllAsync()
        {
            var fileLogsMinhaCdn = GetFileLogs("minha-cdn-logs");
            List<string> fileLogsAgora;
            
            if (fileLogsMinhaCdn.Count > 0)
                fileLogsAgora = GetFileLogs("agora-logs");
            else
                fileLogsAgora = new List<string>();

            var fileLogsFull = new List<FileLog>();

            foreach (var fileLog in fileLogsMinhaCdn) 
            {
                var contentTextFileLogMinhaCdn = await _handlerIOFile.ReadToFileAsync(fileLog);

                var idFileLog = _infoFileLog.GetFileId(fileLog);

                var contentFileLogAgoraById = fileLogsAgora.FirstOrDefault(file => int.Parse(Regex.Match(Path.GetFileName(file), _infoFileLog.GetRegexFileNameId()).Value) == idFileLog);

                var contentTextFileLogAgora = !string.IsNullOrWhiteSpace(contentFileLogAgoraById) ? await _handlerIOFile.ReadToFileAsync(contentFileLogAgoraById) : string.Empty;

                var fileLogFull = new FileLog(idFileLog, contentTextFileLogMinhaCdn, contentTextFileLogAgora);
                
                fileLogsFull.Add(fileLogFull);
            }

            return fileLogsFull;
        }

        private string GetPathFileLog(string tag, long id)
        {
            var physicalPath = _infoFileLog.GetPhysicalPath(tag);
            var fullPath = Path.GetFullPath(physicalPath);

            var regexFileName = _infoFileLog.GetRegexFileName();
            var regexFileNameId = _infoFileLog.GetRegexFileNameId();

            var pathFileLog = Directory.GetFiles(fullPath)
                    .FirstOrDefault(file => int.Parse(Regex.Match(Path.GetFileName(file), _infoFileLog.GetRegexFileNameId()).Value) == id);

            return pathFileLog;
        }

        public async Task<FileLog> GetByIdAsync(long id)
        {
            string contentTextFileLogMinhaCdn = string.Empty;
            string contentTextFileAgora = string.Empty;

            var pathFileLogMinhaCdn = GetPathFileLog("minha-cdn-logs", id);
            contentTextFileLogMinhaCdn = !string.IsNullOrWhiteSpace(pathFileLogMinhaCdn) ? await _handlerIOFile.ReadToFileAsync(pathFileLogMinhaCdn) : string.Empty;

            if (!string.IsNullOrWhiteSpace(contentTextFileLogMinhaCdn))
            {
                var pathFileLogAgora = GetPathFileLog("agora-logs", id);
                contentTextFileAgora = !string.IsNullOrWhiteSpace(pathFileLogAgora) ? await _handlerIOFile.ReadToFileAsync(pathFileLogAgora) : string.Empty;
            }

            if (string.IsNullOrWhiteSpace(contentTextFileLogMinhaCdn) || string.IsNullOrWhiteSpace(contentTextFileAgora))
                return null;

            var fileLog = new FileLog(id, contentTextFileLogMinhaCdn, contentTextFileAgora);

            return fileLog;
        }

        public long GetNewFileId()
        {
            var tag = "agora-logs";
            var physicalPath = _infoFileLog.GetPhysicalPath(tag);
            var fullPath = Path.GetFullPath(physicalPath);

            var regexFileName = _infoFileLog.GetRegexFileName();
            var regexFileNameId = _infoFileLog.GetRegexFileNameId();

            var contentLastFileLog = Directory.GetFiles(fullPath)
                    .Where(file => Regex.IsMatch(Path.GetFileName(file), regexFileName))
                    .OrderByDescending(file => int.Parse(Regex.Match(Path.GetFileName(file), regexFileNameId).Value))
                    .FirstOrDefault();


            var fileId = !string.IsNullOrWhiteSpace(contentLastFileLog) ? _infoFileLog.GetFileId(contentLastFileLog) + 1 : 1;

            return fileId;
        }
    }
}
