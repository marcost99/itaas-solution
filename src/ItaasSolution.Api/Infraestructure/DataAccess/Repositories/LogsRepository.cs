using ItaasSolution.Api.Domain.Entities;
using ItaasSolution.Api.Domain.Repositories.Logs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.DataAccess.Repositories
{
    public class LogsRepository : ILogsWriteOnlyRepository, ILogsReadOnlyRepository
    {
        private readonly ItaasSolutionDbContext _dbContext;
        public LogsRepository(ItaasSolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(ItaasSolution.Api.Domain.Entities.Log log)
        {
            await _dbContext.Log.AddAsync(log);
        }

        public async Task<List<Log>> GetAllAsync()
        {
            return await _dbContext.Log.AsNoTracking().ToListAsync();
        }

        public async Task<Log> GetByIdAsync(long id)
        {
            return await _dbContext.Log.AsNoTracking().FirstOrDefaultAsync(log => log.Id == id);
        }
    }
}
