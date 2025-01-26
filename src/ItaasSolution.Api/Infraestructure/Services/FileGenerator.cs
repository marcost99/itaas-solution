using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services
{
    public class FileGenerator : IFileGenerator
    {
        private readonly IConfiguration _configuration;
        public FileGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method converts the datas to the format Agora and saves a file
        public async Task<bool> FileGeneratorAsync(string textFile, string fileName)
        {
            var fileGenerated = false;

            var fileRepositories = _configuration.GetSection("Settings:FileRepository").Get<List<Dictionary<string, string>>>();
            var physicalPath = fileRepositories
                .FirstOrDefault(repo => repo["Tag"] == "agora-logs")?["PhysicalPath"];
            var agoraLogsPath = Path.GetFullPath(physicalPath);

            var file = Path.Combine(agoraLogsPath, fileName);

            await WriteToFileAsync(textFile, file);

            if (File.Exists(file))
                fileGenerated = true;

            return fileGenerated;
        }

        // This method saves a file
        private async Task WriteToFileAsync(string conteudo, string caminhoArquivo)
        {
            using (StreamWriter writer = new StreamWriter(caminhoArquivo))
            {
                await writer.WriteAsync(conteudo);
            }
        }
    }
}
