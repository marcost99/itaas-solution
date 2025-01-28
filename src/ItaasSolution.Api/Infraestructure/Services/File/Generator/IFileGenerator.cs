using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services.File.Generator
{
    public interface IFileGenerator
    {
        Task<bool> FileGeneratorAsync(string textFile, string physicalPath, string fileName);
    }
}
