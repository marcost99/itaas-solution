using ItaasSolution.Api.Communication.Requests.FileLog;
using ItaasSolution.Api.Communication.Responses.FileLog;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.FileLog.Converter
{
    public interface IConverterFileLogUseCase
    {
        Task<ResponseConverterFileLogJson> ExecuteAsync(RequestConverterFileLogJson request);
    }
}
