using System.Threading.Tasks;
using System;

namespace ItaasSolution.Api.Infraestructure.Services.Cache
{
    public interface ICacheService
    {
        Task<(string cacheStatus, T data)> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> getDataFunc, TimeSpan cacheDuration);
        void InvalidateCache(string cacheKey);
    }
}
