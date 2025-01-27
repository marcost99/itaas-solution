using ItaasSolution.Api.Communication.Responses;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.GetAll
{
    public interface IGetAllLogUseCase
    {
        Task<ResponseLogsJson> ExecuteAsync();
    }
}
