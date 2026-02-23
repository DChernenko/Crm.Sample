using Crm.Sample.Application.Common.Enums;
using Crm.Sample.Application.Common.Interfaces;
using Crm.Sample.Infrastructure.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Crm.Sample.Infrastructure.Services.Redis
{
    public class RedisCache : IApplicationCache
    {
        private readonly RedisOptions _redisOptions;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisCache(IConnectionMultiplexer connectionMultiplexer, IOptionsSnapshot<RedisOptions> options)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _redisOptions = options.Value;
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

            if (expiry == null)
                expiry = TimeSpan.FromMinutes(_redisOptions.DefaulExpirationInMinutes);

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

        public string GetCacheKey(CacheOperation operation, string entity, params object[] parameters)
        {
            var key = $"{entity}:{operation}";
            if (parameters != null && parameters.Length > 0)
            {
                key += ":" + string.Join(":", parameters);
            }
            return key;
        }
    }
}
