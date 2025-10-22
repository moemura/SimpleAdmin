
namespace SimpleAdmin.Services.Catches
{
    public class NoCacheService : ICacheService
    {
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            return await factory();
        }

        public Task RemoveAsync(string key)
        {
            return Task.CompletedTask;
        }

        public Task RemoveByPrefixAsync(string prefixKey)
        {
            return Task.CompletedTask;
        }
    }
}
