using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.Services
{
    public interface IFileGenerator
    {
        Task<(bool fileGenerated, string nameFile)> FileGeneratorAsync(string textFile);
    }
}
