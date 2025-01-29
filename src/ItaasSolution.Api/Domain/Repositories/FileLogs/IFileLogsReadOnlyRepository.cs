using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Domain.Repositories.FileLogs
{
    public interface IFileLogsReadOnlyRepository
    {
        Task<List<ItaasSolution.Api.Domain.Entities.FileLog>> GetAllAsync();
        Task<ItaasSolution.Api.Domain.Entities.FileLog> GetByIdAsync(long id);
        long GetNewFileId();
    }
}
