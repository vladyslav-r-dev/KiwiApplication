using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace KiwiApp.Application.Services;

public class CacheService(IDistributedCache cache)
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await cache.GetStringAsync(key);
        
        if (json is null)
            return default;

        return JsonSerializer.Deserialize<T>(json);
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var json = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await cache.SetStringAsync(key, json, options);
    }
    
    public async Task RemoveAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}