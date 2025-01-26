using ItaasSolution.Api.Application.Validations.Log;
using ItaasSolution.Api.Communication.Requests;
using ItaasSolution.Api.Communication.Responses;
using ItaasSolution.Api.Domain.Repositories;
using ItaasSolution.Api.Domain.Repositories.Logs;
using ItaasSolution.Api.Exception.ExceptionsBase;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Application.UseCases.Log.Register
{
    public class RegisterLogUseCase : IRegisterLogUseCase
    {
        private readonly ILogsWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterLogUseCase(ILogsWriteOnlyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRegisterLogJson> ExecuteAsync(RequestLogJson request)
        {
            // validates the datas of request
            Validate(request);

            // create a new object Log and set values of object request to object Log
            var entity = new ItaasSolution.Api.Domain.Entities.Log(
                request.HtttpMethod,
                request.StatusCode,
                request.UriPath,
                request.TimeTaken,
                request.ResponseSize,
                request.CacheStatus,
                DateTime.UtcNow
            );

            await _repository.AddAsync(entity);

            await _unitOfWork.Commit();

            return new ResponseRegisterLogJson() { Id = entity.Id };
        }

        private void Validate(RequestLogJson request)
        {
            var validator = new RequestLogJsonValidator();
            var result = validator.Validate(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
