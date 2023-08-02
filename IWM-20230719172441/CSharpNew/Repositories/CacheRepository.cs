using IWM.Common;
using StackExchange.Redis;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TrueSight;
using TrueSight.Caching;

namespace IWM.Repositories
{
    public class CacheRepository
    {
        private readonly ICurrentContext CurrentContext;        
        private readonly IDatabase Database;
        private readonly IServer Server;

        public CacheRepository(ICurrentContext CurrentContext, IRedisStore RedisStore)
        {
            this.CurrentContext = CurrentContext;
            Database = RedisStore.GetDatabase();
            Server = RedisStore.GetServer();
        }

        private string BuildKey(string key)
        {
            return $"{CurrentContext.TenantId}|{StaticParams.ModuleName}|{key}";
        }
        public async Task SetToCache<T>(string key, T data, TimeSpan? expiry = null)
        {
            try
            {
                await Database.StringSetAsync(BuildKey(key), JsonConvert.SerializeObject(data), expiry, flags: CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetToCache:" + ex.Message);
            }
        }

        public async Task<T> GetFromCache<T>(string key)
        {
            try
            {
                var data = await Database.StringGetAsync(BuildKey(key));
                if (string.IsNullOrEmpty(data))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFromCache:" + ex.Message);
                return default(T);
            }
        }

        public async Task RemoveCache(string key)
        {
            try
            {
                await Database.KeyDeleteAsync(BuildKey(key), CommandFlags.FireAndForget);
            }
            catch (Exception ex)
            {
                Console.WriteLine("RemoveCache:" + ex.Message);
            }
        }

        public async Task RemoveCacheByPrefixKey(string prefixKey)
        {
            try
            {
                prefixKey = BuildKey(prefixKey);
                var RedisKeys = Server.Keys(Database.Database, $"{prefixKey}*");
                foreach (var k in RedisKeys)
                {
                    await Database.KeyDeleteAsync(k, CommandFlags.FireAndForget);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RemoveCacheByPrefixKey:" + ex.Message);
            }
        }
    }
}
