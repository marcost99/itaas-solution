using ItaasSolution.Api.Communication.Responses.Log;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.GetById
{
    public interface IGetByIdLogUseCase
    {
        Task<ResponseLogJson> ExecuteAsync(long id);
    }
}
