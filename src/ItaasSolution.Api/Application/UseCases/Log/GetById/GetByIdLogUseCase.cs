using ItaasSolution.Api.Exception.ExceptionsBase;
using ItaasSolution.Api.Exception;
using System.Threading.Tasks;
using ItaasSolution.Api.Domain.Repositories.Logs;
using ItaasSolution.Api.Communication.Responses.Log;

namespace ItaasSolution.Api.Application.UseCases.Log.GetById
{
    public class GetByIdLogUseCase : IGetByIdLogUseCase
    {
        private readonly ILogsReadOnlyRepository _repository;
        public GetByIdLogUseCase(ILogsReadOnlyRepository repository)
        {
            _repository = repository;;
        }
        public async Task<ResponseLogJson> ExecuteAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(ResourceErrorMessages.LOG_NOT_FOUND);
            }

            var response = new ResponseLogJson();
            response.Id = entity.Id;
            response.HtttpMethod = entity.HtttpMethod;
            response.StatusCode = entity.StatusCode;
            response.UriPath = entity.UriPath;
            response.TimeTaken = entity.TimeTaken;
            response.ResponseSize = entity.ResponseSize;
            response.CacheStatus = entity.CacheStatus;
            response.DateCreation = entity.DateCreation;

            return response;
        }
    }
}
