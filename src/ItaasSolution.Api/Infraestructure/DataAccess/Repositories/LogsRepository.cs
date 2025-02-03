using ItaasSolution.Api.Domain.Entities;
using ItaasSolution.Api.Domain.Repositories.Logs;
using ItaasSolution.Api.Infraestructure.Services.Cache;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.DataAccess.Repositories
{
    public class LogsRepository : ILogsWriteOnlyRepository, ILogsReadOnlyRepository
    {
        private readonly ItaasSolutionDbContext _dbContext;
        private readonly ICacheService _cacheService;
        private const string _cacheKey = "logs-get-all";

        public LogsRepository(ItaasSolutionDbContext dbContext, ICacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }
        public async Task AddAsync(ItaasSolution.Api.Domain.Entities.Log log)
        {
            await _dbContext.Log.AddAsync(log);
        }

        public async Task<(string cacheStatus, List<Log> data)> GetAllAsync()
        {
            return await _cacheService.GetOrSetCacheAsync(_cacheKey, async () =>
            {
                return await _dbContext.Log.AsNoTracking().ToListAsync();
            }, TimeSpan.FromMinutes(1));
        }

        public async Task<Log> GetByIdAsync(long id)
        {
            return await _dbContext.Log.AsNoTracking().FirstOrDefaultAsync(log => log.Id == id);
        }
    }
}
