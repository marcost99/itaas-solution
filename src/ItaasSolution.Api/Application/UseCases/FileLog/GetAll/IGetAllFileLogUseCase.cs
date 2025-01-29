using ItaasSolution.Api.Communication.Responses.FileLog;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.FileLog.GetAll
{
    public interface IGetAllFileLogUseCase
    {
        Task<ResponseConvertedFileLogsJson> ExecuteAsync();
    }
}
