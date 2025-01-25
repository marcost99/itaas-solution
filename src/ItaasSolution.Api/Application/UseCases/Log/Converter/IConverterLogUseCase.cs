using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Converter
{
    public interface IConverterLogUseCase
    {
        Task<ResponseConverterLogJson> Execute(RequestConverterLogJson request);
    }
}
