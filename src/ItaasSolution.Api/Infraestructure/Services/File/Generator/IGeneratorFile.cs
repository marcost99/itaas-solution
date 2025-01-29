using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services.File.Generator
{
    public interface IGeneratorFile
    {
        Task<bool> FileGeneratorAsync(string textFile, string physicalPath, string fileName);
    }
}
