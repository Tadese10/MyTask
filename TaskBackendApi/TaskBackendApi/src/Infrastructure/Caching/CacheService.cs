using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Caching;
public class CacheService : ICacheService
{
    private readonly IDatabase _redisDb;
#pragma warning disable S4487 // Unread "private" fields should be removed
    private readonly ILogger<CacheService> _logger;
#pragma warning restore S4487 // Unread "private" fields should be removed

    public CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger)
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }

    public T? Get<T>(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default)
    {
        try
        {
            RedisValue value = await _redisDb.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }
        catch (Exception)
        {
            return default;
        }
    }

    public void Refresh(string key)
    {
        throw new NotImplementedException();
    }

    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
       _ =  await _redisDb.KeyDeleteAsync(key); 
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        throw new NotImplementedException();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
    {
        await _redisDb.StringSetAsync(key, JsonConvert.SerializeObject(value));
    }
}
