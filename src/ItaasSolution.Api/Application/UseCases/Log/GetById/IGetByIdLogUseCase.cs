using ItaasSolution.Api.Communication.Responses;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.GetById
{
    public interface IGetByIdLogUseCase
    {
        Task<ResponseLogJson> ExecuteAsync(long id);
    }
}
