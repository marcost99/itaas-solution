using ItaasSolution.Api.Domain.Entities;
using ItaasSolution.Api.Domain.Repositories.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItaasSolution.Api.Infraestructure.DataAccess.Repositories
{
    public class LogsRepository : ILogsWriteOnlyRepository, ILogsReadOnlyRepository
    {
        private readonly ItaasSolutionDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private const string _cacheKey = "logs-get-all";

        public LogsRepository(ItaasSolutionDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        public async Task AddAsync(ItaasSolution.Api.Domain.Entities.Log log)
        {
            await _dbContext.Log.AddAsync(log);
        }

        public async Task<(string cacheStatus, List<Log> data)> GetAllAsync()
        {
            if (_memoryCache.TryGetValue(_cacheKey, out List<Log> cachedData))
            {
                return ("HIT", cachedData);
            }

            var repositoryData = await _dbContext.Log.AsNoTracking().ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _memoryCache.Set(_cacheKey, repositoryData, cacheEntryOptions);

            return ("MISS", repositoryData);
        }

        public async Task<Log> GetByIdAsync(long id)
        {
            return await _dbContext.Log.AsNoTracking().FirstOrDefaultAsync(log => log.Id == id);
        }
    }
}
