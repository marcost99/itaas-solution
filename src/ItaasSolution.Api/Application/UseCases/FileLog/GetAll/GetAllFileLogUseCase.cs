using ItaasSolution.Api.Communication.Responses.FileLog;
using ItaasSolution.Api.Domain.Repositories.FileLogs;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.FileLog.GetAll
{
    public class GetAllFileLogUseCase : IGetAllFileLogUseCase
    {
        private readonly IFileLogsReadOnlyRepository _repository;

        public GetAllFileLogUseCase(IFileLogsReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseConvertedFileLogsJson> ExecuteAsync()
        {
            var entities = await _repository.GetAllAsync();

            var response = new ResponseConvertedFileLogsJson();

            foreach (var entity in entities)
            {
                var fileLog = new ResponseConvertedFileLogJson();
                fileLog.Id = entity.Id;
                fileLog.ContentTextFileLogMinhaCdn = entity.ContentTextFileLogMinhaCdn;
                fileLog.ContentTextFileLogAgora = entity.ContentTextFileLogAgora;

                response.FileLogs.Add(fileLog);
            }

            return response;
        }
    }
}
