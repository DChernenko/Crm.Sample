using Crm.Sample.Application.Common.Enums;

namespace Crm.Sample.Application.Common.Interfaces
{
    public interface IApplicationCach
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task RemoveAsync(string[] key);
        Task<bool> KeyExistsAsync(string key);
        string GetCacheKey(CacheOperation operation, string entity, params object[] parameters);
    }
}
