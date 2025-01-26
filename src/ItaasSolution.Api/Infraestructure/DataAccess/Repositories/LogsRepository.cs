using ItaasSolution.Api.Domain.Repositories.Logs;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.DataAccess.Repositories
{
    public class LogsRepository : ILogsWriteOnlyRepository
    {
        private readonly ItaasSolutionDbContext _dbContext;
        public LogsRepository(ItaasSolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(ItaasSolution.Api.Domain.Entities.Log log)
        {
            await _dbContext.Log.AddAsync(log);
        }
    }
}
