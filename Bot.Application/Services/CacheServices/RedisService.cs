using Bot.Application.Interfaces;
using IDatabase = StackExchange.Redis.IDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

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

        public async Task SetAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
            await _db.KeyExpireAsync(key, TimeSpan.FromDays(1));
        }
    }
}
