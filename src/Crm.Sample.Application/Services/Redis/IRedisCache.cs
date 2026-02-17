namespace Crm.Sample.Application.Services.Redis
{
    // todo where is located in infrastructure or application?
    public interface IRedisCache
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task RemoveAsync(string[] key);
        Task<bool> KeyExistsAsync(string key);
    }
}
