using System.IO;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services.File.Generator
{
    public class FileGenerator : IFileGenerator
    {
        // This method converts the datas to the format Agora and saves a file
        public async Task<bool> FileGeneratorAsync(string textFile, string physicalPath, string fileName)
        {
            var fileGenerated = false;

            var fullPath = Path.GetFullPath(physicalPath);

            var file = Path.Combine(fullPath, fileName);

            await WriteToFileAsync(textFile, file);

            if (System.IO.File.Exists(file))
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
