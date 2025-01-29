using ItaasSolution.Api.Communication.Responses.FileLog;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.FileLog.GetById
{
    public interface IGetByIdFileLogUseCase
    {
        Task<ResponseConvertedFileLogJson> ExecuteAsync(long id);
    }
}
