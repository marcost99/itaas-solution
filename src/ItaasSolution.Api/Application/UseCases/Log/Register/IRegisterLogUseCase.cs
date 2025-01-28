using ItaasSolution.Api.Communication.Requests.Log;
using ItaasSolution.Api.Communication.Responses.Log;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Register
{
    public interface IRegisterLogUseCase
    {
        Task<ResponseRegisterLogJson> ExecuteAsync(RequestLogJson request);
    }
}
