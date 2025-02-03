using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;

namespace ItaasSolution.Api.Infraestructure.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<(string cacheStatus, T data)> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc, TimeSpan cacheDuration)
        {
            if (_memoryCache.TryGetValue(cacheKey, out T cachedData))
            {
                return ("HIT", cachedData);
            }

            var data = await getDataFunc();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(cacheDuration);

            _memoryCache.Set(cacheKey, data, cacheEntryOptions);

            return ("MISS", data);
        }

        public void InvalidateCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}
