using ItaasSolution.Api.Communication.Responses.FileLog;
using ItaasSolution.Api.Domain.Repositories.FileLogs;
using ItaasSolution.Api.Exception;
using ItaasSolution.Api.Exception.ExceptionsBase;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.FileLog.GetById
{
    public class GetByIdFileLogUseCase : IGetByIdFileLogUseCase
    {
        private readonly IFileLogsReadOnlyRepository _repository;
        public GetByIdFileLogUseCase(IFileLogsReadOnlyRepository repository)
        {
            _repository = repository; ;
        }
        public async Task<ResponseConvertedFileLogJson> ExecuteAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(ResourceErrorMessages.FILE_LOG_NOT_FOUND);
            }

            var response = new ResponseConvertedFileLogJson();
            response.Id = entity.Id;
            response.ContentTextFileLogMinhaCdn = entity.ContentTextFileLogMinhaCdn;
            response.ContentTextFileLogAgora = entity.ContentTextFileLogAgora;

            return response;
        }
    }
}
