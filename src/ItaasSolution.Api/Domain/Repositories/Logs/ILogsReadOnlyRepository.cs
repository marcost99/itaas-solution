using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Domain.Repositories.Logs
{
    public interface ILogsReadOnlyRepository
    {
        Task<List<ItaasSolution.Api.Domain.Entities.Log>> GetAllAsync();
        Task<ItaasSolution.Api.Domain.Entities.Log> GetByIdAsync(long id);
    }
}
