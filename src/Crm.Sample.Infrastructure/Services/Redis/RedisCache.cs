using Crm.Sample.Application.Services.Redis;
using StackExchange.Redis;
using System.Text.Json;

namespace Crm.Sample.Infrastructure.Services.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisCache(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);

            if (!value.HasValue)
                return default;

            return JsonSerializer.Deserialize<T>((string)value!);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serialized = JsonSerializer.Serialize(value);
            return _database.StringSetAsync(key, serialized, expiry, When.Always);
        }

        public async Task RemoveAsync(string[] keys)
        {
            if (keys == null || keys.Length == 0)
                return;

            try
            {
                await Task.WhenAll(keys.Select(key => _database.KeyDeleteAsync(key)));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to invalidate cache for keys: {string.Join(", ", keys.Where(k => !string.IsNullOrEmpty(k)))}", ex);
            }
        }

        public Task<bool> KeyExistsAsync(string key)
            => _database.KeyExistsAsync(key);

        public Task HashSetAsync(string key, string field, string value)
            => _database.HashSetAsync(key, field, value);

        public async Task<string> HashGetAsync(string key, string field)
           => await _database.HashGetAsync(key, field);
    }
}
