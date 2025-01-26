using System.Threading.Tasks;

namespace ItaasSolution.Api.Domain.Repositories.Logs
{
    public interface ILogsWriteOnlyRepository
    {
        Task AddAsync(ItaasSolution.Api.Domain.Entities.Log log);
    }
}
