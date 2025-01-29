using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services.File.HandlerIO
{
    public interface IHandlerIOFile
    {
        Task<string> ReadToFileAsync(string pathFile);
        Task WriteToFileAsync(string content, string pathFile);
    }
}
