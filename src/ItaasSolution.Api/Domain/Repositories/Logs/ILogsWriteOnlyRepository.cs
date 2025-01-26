using System.Threading.Tasks;

namespace ItaasSolution.Api.Domain.Repositories.Logs
{
    public interface ILogsWriteOnlyRepository
    {
        Task Add(ItaasSolution.Api.Domain.Entities.Log log);
    }
}
