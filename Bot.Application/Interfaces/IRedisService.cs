using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Interfaces
{
    public interface IRedisService
    {
        Task<string?> GetAsync(string key);
        Task SetAsync(string key, string value);
        Task DeleteAsync(string key);
        Task<T?> GetObjectAsync<T>(string key) where T: class;
        Task SetObjectAsync<T>(string key, T obj) where T : class;
    }
}
