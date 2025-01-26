using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Register
{
    public interface IRegisterLogUseCase
    {
        Task<ResponseRegisterLogJson> ExecuteAsync(RequestLogJson request);
    }
}
