using System;
using System.IO;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services
{
    public class FileGenerator : IFileGenerator
    {
        // This method converts the datas to the format Agora and saves a file
        public async Task<(bool fileGenerated, string nameFile)> FileGeneratorAsync(string textFile, string prefixNameFile)
        {
            var fileGenerated = false;

            var solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var repositoryPath = Path.Combine(solutionDirectory, "repository\\logs\\");

            var now = DateTime.UtcNow;
            var nameFile = @"" + prefixNameFile + "_" + $"{now.Year}{now.Month:D2}{now.Day:D2}{now.Hour:D2}{now.Minute:D2}{now.Second:D2}{now.Millisecond:D3}.txt";
            var file = repositoryPath + nameFile;

            await WriteToFileAsync(textFile, file);

            if (File.Exists(file))
                fileGenerated = true;

            return (fileGenerated, nameFile);
        }

        // This method saves a file
        public async Task WriteToFileAsync(string conteudo, string caminhoArquivo)
        {
            using (StreamWriter writer = new StreamWriter(caminhoArquivo))
            {
                await writer.WriteAsync(conteudo);
            }
        }
    }
}
