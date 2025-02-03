using ItaasSolution.Api.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Domain.Repositories.Logs
{
    public interface ILogsReadOnlyRepository
    {
        Task<(string cacheStatus, List<Log> data)> GetAllAsync();
        Task<Log> GetByIdAsync(long id);
    }
}
