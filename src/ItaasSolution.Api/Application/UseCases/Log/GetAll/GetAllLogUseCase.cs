using ItaasSolution.Api.Communication.Responses.Log;
using ItaasSolution.Api.Domain.Repositories.Logs;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.GetAll
{
    public class GetAllLogUseCase : IGetAllLogUseCase
    {
        private readonly ILogsReadOnlyRepository _repository;

        public GetAllLogUseCase(ILogsReadOnlyRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseLogsJson> ExecuteAsync()
        {
            var entities = await _repository.GetAllAsync();

            var response = new ResponseLogsJson();

            foreach (var entity in entities) 
            {
                var log = new ResponseShortLogJson();
                log.Id = entity.Id;
                log.HtttpMethod = entity.HtttpMethod;
                log.StatusCode = entity.StatusCode;
                log.UriPath = entity.UriPath;

                response.Logs.Add(log);
            }

            return response;
        }
    }
}
