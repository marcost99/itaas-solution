using System.IO;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services.File.HandlerIO
{
    public class HandlerIOFile : IHandlerIOFile
    {
        // This method reads a file
        public async Task<string> ReadToFileAsync(string pathFile)
        {
            string content;
            using (var reader = new StreamReader(pathFile))
            {
                content = await reader.ReadToEndAsync();
            }
            return content;
        }

        // This method saves a file
        public async Task WriteToFileAsync(string content, string pathFile)
        {
            using (StreamWriter writer = new StreamWriter(pathFile))
            {
                await writer.WriteAsync(content);
            }
        }
    }
}
