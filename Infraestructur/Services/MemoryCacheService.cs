using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Services
{
    public class MemoryCacheService: ICacheService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public T Get<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration,
                Priority = CacheItemPriority.Normal // CHECKING IF WORKS
            };

            _cache.Set(key, value, options);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
