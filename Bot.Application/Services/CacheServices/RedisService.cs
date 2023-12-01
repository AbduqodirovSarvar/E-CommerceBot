using Bot.Application.Interfaces;
using IDatabase = StackExchange.Redis.IDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Bot.Application.Services.CacheServices
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _db;
        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task DeleteAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<string?> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value;
        }

        public async Task<T?> GetObjectAsync<T>(string key) where T : class
        {
            var value = await _db.StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T?>(value!);
            }
            return null;
        }

        public async Task SetAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
            await _db.KeyExpireAsync(key, TimeSpan.FromDays(1));
        }

        public async Task SetObjectAsync<T>(string key, T obj) where T : class
        {
            var stringJson = JsonConvert.SerializeObject(obj);
            await _db.StringSetAsync(key, stringJson);
            await _db.KeyExpireAsync(key, TimeSpan.FromDays(1));
        }
    }
}
