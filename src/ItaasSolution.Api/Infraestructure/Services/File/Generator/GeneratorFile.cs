using ItaasSolution.Api.Infraestructure.Services.File.HandlerIO;
using System.IO;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services.File.Generator
{
    public class GeneratorFile : IGeneratorFile
    {
        private readonly IHandlerIOFile _handlerIOFile;

        public GeneratorFile(IHandlerIOFile handlerIOFile)
        {
            _handlerIOFile = handlerIOFile;
        }

        // This method converts the datas to the format Agora and saves a file
        public async Task<bool> FileGeneratorAsync(string textFile, string physicalPath, string fileName)
        {
            var fileGenerated = false;

            var fullPath = Path.GetFullPath(physicalPath);

            var file = Path.Combine(fullPath, fileName);

            await _handlerIOFile.WriteToFileAsync(textFile, file);

            if (System.IO.File.Exists(file))
                fileGenerated = true;

            return fileGenerated;
        }
    }
}
