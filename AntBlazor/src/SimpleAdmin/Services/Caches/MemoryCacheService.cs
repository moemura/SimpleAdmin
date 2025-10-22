using Microsoft.Extensions.Caching.Memory;

namespace SimpleAdmin.Services.Catches
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private readonly Dictionary<string, DateTime> _cacheKeys;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            _cacheKeys = new Dictionary<string, DateTime>();
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                if (!_cacheKeys.ContainsKey(key))
                    _cacheKeys[key] = DateTime.UtcNow;
                return cachedValue;
            }

            var value = await factory();

            var options = expiration.HasValue
                ? new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiration.Value)
                    .SetAbsoluteExpiration(expiration.Value.Add(TimeSpan.FromMinutes(5)))
                : _cacheOptions;

            _memoryCache.Set(key, value, options);
            _cacheKeys[key] = DateTime.UtcNow;

            return value;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            _cacheKeys.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByPrefixAsync(string prefixKey)
        {
            var keysToRemove = _cacheKeys.Keys.Where(k => k.StartsWith(prefixKey)).ToList();
            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                _cacheKeys.Remove(key);
            }
            return Task.CompletedTask;
        }
    }
} 