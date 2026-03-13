using System.Text.Json;

using StackExchange.Redis;

namespace athlon_alpha_be.api.Services;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IConnectionMultiplexer muxer, ILogger<RedisService> logger)
    {
        _database = muxer.GetDatabase();
        _logger = logger;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        string json = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, json, expiry, When.Always);
        _logger.LogInformation("Set value in Redis. Key: {Key}, Expiry:{expiry}", key, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        RedisValue value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            return default;
        }

        _logger.LogInformation("Retrieved value from Redis. Key: {Key}", key);
        return JsonSerializer.Deserialize<T>(value.ToString()!);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
        _logger.LogInformation("Removed value from Redis. Key: {Key}", key);
    }
}
