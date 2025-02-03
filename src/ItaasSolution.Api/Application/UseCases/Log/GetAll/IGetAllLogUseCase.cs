using ItaasSolution.Api.Communication.Responses.Log;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.GetAll
{
    public interface IGetAllLogUseCase
    {
        Task<(string cacheStatus, ResponseLogsJson data)> ExecuteAsync();
    }
}
