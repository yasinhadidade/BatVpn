using StackExchange.Redis;

namespace BatVpn.Infrastructure.Redis
{
    public interface IRedisClient
    {
        Task<bool> Set(int database, string key, string value, TimeSpan? expire = null);
        Task<string> Get(int database, string key);
        Task<string> GetSet(int database, string key, string value, TimeSpan? expire = null);
        Task<bool> Delete(int database, string key);
        Task<long> Increment(int database, string key, long value);
        Task<long> Increment(int database, string key);
        Task<double> Increment(int database, string key, double value);
        Task<bool> HSet(int database, string hash, string key, string value);
        Task<string> HGet(int database, string hash, string key);
        Task<string> Pop(int redisDatabase, string hash);
        Task<long> Push(int redisDatabase, string hash, string value);
        Task<long> PushLeft(int redisDatabase, string hash, string value);
        List<string> Range(int redisDatabase, string hash, int count);
        Task<double> HIncrement(int database, string hash, string key, double value);
        Task<Dictionary<string, string>> HGetAll(int database, string hash);
        Task<int> GetTimeToLive(int database, string key);
        Task<bool> Expire(int database, string key, TimeSpan expire);
        Task<bool> Move(int database, int destinationDatabase, string key);
        List<string> Scan(int database, int pagesize, string pattern = "");
        Task<long> Count(int database);
        ISubscriber GetSubscriber();
    }
}
