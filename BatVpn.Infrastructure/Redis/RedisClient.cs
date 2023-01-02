using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace BatVpn.Infrastructure.Redis
{
    public enum RedisDatabases
    {
        LoginCodes,
        SignedOutTokens,
        PagesIds,
        Instagram,
        MediaToDownload,
        instagramOtp
    }
    enum RedisSettingsEnums
    {
        Redis,
        Host,
        Port
    }
    public class RedisSettings
    {
        public static string Redis { get; protected set; } = Enum.GetName(typeof(RedisSettingsEnums), RedisSettingsEnums.Redis);
        public static string Host { get; protected set; } = Enum.GetName(typeof(RedisSettingsEnums), RedisSettingsEnums.Host);
        public static string Port { get; protected set; } = Enum.GetName(typeof(RedisSettingsEnums), RedisSettingsEnums.Port);
    }
    internal class RedisDatabase : Iint
    {
        public IDatabase Database { get; }

        public int Number { get; }

        public RedisDatabase(IDatabase database, int number)
        {
            Database = database;
            Number = number;
        }
    }
    public class RedisClient : IRedisClient
    {
        private readonly IConfiguration _configuration;

        private readonly IRedisFrameworkFacade redisFrameworkFacade;

        private readonly ConcurrentDictionary<int, Iint> databaseMap;

        public RedisClient(IRedisFrameworkFacade redisFrameworkFacade)
        {
            this.redisFrameworkFacade = redisFrameworkFacade;
            databaseMap = new ConcurrentDictionary<int, Iint>();
        }

        public RedisClient(IConfiguration configuration)
            : this(new RedisFrameworkFacade())
        {
            _configuration = configuration;
            ConnectIfNotConnected();
        }

        public void ConnectIfNotConnected()
        {
            try
            {
                string host = _configuration.GetSection($"{RedisSettings.Redis}:{RedisSettings.Host}").Value;
                string port = _configuration.GetSection($"{RedisSettings.Redis}:{RedisSettings.Port}").Value;
                if (host.Contains(",") && port.Contains(","))
                {
                    string[] hosts = host.Split(",").Select(a => a.Trim()).ToArray();
                    int[] ports = port.Split(",").Select(a => int.Parse(a.Trim())).ToArray();
                    if (hosts.Length != ports.Length)
                        throw new Exception("endpoints are invalid!");
                    redisFrameworkFacade.Connect(hosts, ports);
                }
                else
                    redisFrameworkFacade.Connect(host, Convert.ToInt32(port));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Iint GetDatabase(int database)
        {
            if (!databaseMap.ContainsKey(database))
            {
                var redisRedisDatabaseWrapper = redisFrameworkFacade.GetDatabase(database);
                databaseMap[database] = redisRedisDatabaseWrapper;
            }
            return databaseMap[database];
        }
        public ISubscriber GetSubscriber()
        {
            return redisFrameworkFacade.GetSubscriber();
        }
        public async Task<bool> Set(int database, string key, string value, TimeSpan? expire = null)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.SetKey(redisRedisDatabaseWrapper, key, value, expire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// find key and return the value 
        /// (return null if key doesn't exist)
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> Get(int database, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.GetKey(redisRedisDatabaseWrapper, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetSet(int database, string key, string value, TimeSpan? expire = null)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.GetSetKey(redisRedisDatabaseWrapper, key, value, expire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Delete a key from Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Delete(int database, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.DeleteKey(redisRedisDatabaseWrapper, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment value of a key in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> Increment(int database, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.IncrementKey(redisRedisDatabaseWrapper, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+N) value of a key in Redis (N is integer)
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> Increment(int database, string key, long value)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.IncrementKey(redisRedisDatabaseWrapper, key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+N) value of a key in Redis (N is double)
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<double> Increment(int database, string key, double value)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.IncrementKey(redisRedisDatabaseWrapper, key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert a key into a hash table in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> HSet(int database, string hash, string key, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.HSet(redisRedisDatabaseWrapper, hash, key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get a key of hash table in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> HGet(int database, string hash, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.HGet(redisRedisDatabaseWrapper, hash, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Pop a key from a Lis in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> Pop(int database, string hash)
        {
            try
            {
                if (string.IsNullOrEmpty(hash))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.LPop(redisRedisDatabaseWrapper, hash);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Pop a key from a Lis in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> Push(int database, string hash, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.RPush(redisRedisDatabaseWrapper, hash, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Pop a key from a Lis in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> PushLeft(int database, string hash, string value)
        {
            try
            {
                if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.LPush(redisRedisDatabaseWrapper, hash, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> Range(int database, string hash, int count)
        {
            try
            {
                if (string.IsNullOrEmpty(hash) || count == 0)
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return redisFrameworkFacade.LRange(redisRedisDatabaseWrapper, hash, count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+N) a value of a key in a hash table (N is integer)
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<double> HIncrement(int database, string hash, string key, double value)
        {
            try
            {
                if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.HIncrementKey(redisRedisDatabaseWrapper, hash, key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all key-value of a hash table from Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> HGetAll(int database, string hash)
        {
            try
            {
                if (string.IsNullOrEmpty(hash))
                {
                    throw new ArgumentException();
                }
                Dictionary<string, string> result = new Dictionary<string, string>();
                var redisRedisDatabaseWrapper = GetDatabase(database);
                HashEntry[] rows = await redisFrameworkFacade.HGetAll(redisRedisDatabaseWrapper, hash);
                foreach (var row in rows)
                {
                    result.Add(row.Name, row.Value);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get time to live of a key in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<int> GetTimeToLive(int database, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                var timespan = await redisFrameworkFacade.GetTimeToLive(redisRedisDatabaseWrapper, key);
                if (timespan == null)
                {
                    return -1;
                }
                else
                {
                    return (int)timespan.Value.TotalSeconds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set expiration time on a key in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> Expire(int database, string key, TimeSpan expire)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.Expire(redisRedisDatabaseWrapper, key, expire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Move a key-value from a database to another
        /// </summary>
        /// <param name="database"></param>
        /// <param name="destinationDatabase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Move(int database, int destinationDatabase, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentException();
                }
                var redisRedisDatabaseWrapper = GetDatabase(database);
                return await redisFrameworkFacade.Move(redisRedisDatabaseWrapper, destinationDatabase, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all key of a database in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="pagesize"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> Scan(int database, int pagesize, string pattern = "")
        {
            try
            {
                if (pagesize < 0)
                {
                    throw new ArgumentException();
                }
                return redisFrameworkFacade.Scan(database, pagesize, pattern);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get count of key in a redis database
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public async Task<long> Count(int database)
        {
            try
            {
                return await redisFrameworkFacade.Count(database);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    internal class RedisFrameworkFacade : IRedisFrameworkFacade
    {
        private ConnectionMultiplexer connectionMultiplexer;

        /// <summary>
        /// Connect to a Redis endpoint
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connect(string host, int port)
        {
            try
            {
                var configurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = false,
                    ResolveDns = false
                };
                configurationOptions.EndPoints.Add(host, port);
                connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
                //connectionMultiplexer = ConnectionMultiplexer.ConnectAsync(configurationOptions).Result; // await doesn't work! i used ".Result" instead of
            }
            catch (Exception ex)
            {
                throw new ConnectionFailureException(ex);
            }
        }

        /// <summary>
        /// Connect to multiple Redis endpoints
        /// </summary>
        /// <param name="hosts"></param>
        /// <param name="ports"></param>
        public void Connect(string[] hosts, int[] ports)
        {
            try
            {
                var configurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = false,
                    ResolveDns = false
                };
                for (int i = 0; i < hosts.Length; i++)
                {
                    configurationOptions.EndPoints.Add(hosts[i], ports[i]);
                }
                configurationOptions.TieBreaker = "";
                connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
            }
            catch (Exception ex)
            {
                throw new ConnectionFailureException(ex);
            }
        }

        /// <summary>
        /// Return RedisDatabase object based on database number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public Iint GetDatabase(int number)
        {
            return new RedisDatabase(connectionMultiplexer.GetDatabase(number), number);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISubscriber GetSubscriber()
        {
            return connectionMultiplexer.GetSubscriber();
        }
        /// <summary>
        /// Insert new key-value into Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> SetKey(Iint redisDatabase, string key, string value, TimeSpan? expire = null)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.StringSetAsync(key, value, expire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<long> Publish(string channel, string message)
        {
            var sub = connectionMultiplexer.GetSubscriber();
            return await sub.PublishAsync(channel, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="action"></param>
        public void Subscribe(string channel, Action<RedisChannel, RedisValue> action)
        {
            var sub = connectionMultiplexer.GetSubscriber();
            sub.Subscribe(channel, action);
        }
        /// <summary>
        /// Get a value from Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetKey(Iint redisDatabase, string key)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetSetKey(Iint redisDatabase, string key, string value, TimeSpan? expire = null)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.StringGetSetAsync(key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete a key in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKey(Iint redisDatabase, string key)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+1) value of a key in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> IncrementKey(Iint redisDatabase, string key)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.StringIncrementAsync(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+N) value of a key in Redis (N is integer)
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> IncrementKey(Iint redisDatabase, string key, long value)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.StringIncrementAsync(key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+N) value of a key in Redis (N is double)
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<double> IncrementKey(Iint redisDatabase, string key, double value)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.StringIncrementAsync(key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get a key of hash table in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> HGet(Iint redisDatabase, string hash, string key)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.HashGetAsync(hash, key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// LPop a key from a list in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public async Task<string> LPop(Iint redisDatabase, string hash)
        {
            try
            {
                var result = await ((RedisDatabase)redisDatabase).Database.ListLeftPopAsync(hash);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// LPop a key from a list in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public async Task<long> RPush(Iint redisDatabase, string hash, string value)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.ListRightPushAsync(hash, (RedisValue)value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// LPop a key from a list in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public async Task<long> LPush(Iint redisDatabase, string hash, string value)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.ListLeftPushAsync(hash, (RedisValue)value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// LPop a key from a list in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public List<string> LRange(Iint redisDatabase, string hash, int count)
        {
            try
            {
                return ((RedisDatabase)redisDatabase).Database.ListRange(hash, 0, count).Select(a => (string)a).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all key-value of a hash table from Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public async Task<HashEntry[]> HGetAll(Iint redisDatabase, string hash)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.HashGetAllAsync(hash);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert a key into a hash table in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> HSet(Iint redisDatabase, string hash, string key, string value)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.HashSetAsync(hash, key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Increment (+N) a value of a key in a hash table (N is integer)
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<double> HIncrementKey(Iint redisDatabase, string hash, string key, double value)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.HashIncrementAsync(hash, key, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get time to live of a key in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TimeSpan?> GetTimeToLive(Iint redisDatabase, string key)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.KeyTimeToLiveAsync(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set expiration time on a key in Redis
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="key"></param>
        /// <param name="expire"></param>
        /// <returns></returns>
        public async Task<bool> Expire(Iint redisDatabase, string key, TimeSpan expire)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.KeyExpireAsync(key, expire);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Move a key-value from a database to another
        /// </summary>
        /// <param name="redisDatabase"></param>
        /// <param name="destinationDatebase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Move(Iint redisDatabase, int destinationDatebase, string key)
        {
            try
            {
                return await ((RedisDatabase)redisDatabase).Database.KeyMoveAsync(key, destinationDatebase);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get all key of a database in Redis
        /// </summary>
        /// <param name="database"></param>
        /// <param name="pagesize"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> Scan(int database, int pagesize, string pattern = "")
        {
            try
            {
                var result = new List<string>();
                var endpoints = connectionMultiplexer.GetEndPoints();
                var server = GetCurrentServer();
                if (server != null)
                {
                    var keys = server.Keys(database, pattern, pagesize);
                    //IScanningCursor s0 = (IScanningCursor)keys;
                    //result.Cursor = s0.Cursor;
                    foreach (string key in keys)
                    {
                        result.Add(key);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get count of key in a redis database
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public async Task<long> Count(int database)
        {
            try
            {
                var server = GetCurrentServer();
                if (server != null)
                {
                    var count = await server.DatabaseSizeAsync(database);
                    return count;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get active server
        /// در حالتی که ما چند اندپوینت به ردیس معرفی کرده باشیم
        /// چون مدیریت اتصال به اندپوینت ها بر عهده کلاینت ردیس است
        /// در یک لحظه مشخص نمی دانیم که اندپوینت اصلی کدام است
        /// در نتیجه از این روش استفاده کردم
        /// </summary>
        /// <returns></returns>
        private IServer GetCurrentServer()
        {
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                if (server.IsConnected)
                {
                    return server;
                }
            }
            return null;
        }
    }
    public interface IRedisFrameworkFacade
    {
        void Connect(string host, int port);
        void Connect(string[] hosts, int[] ports);

        Iint GetDatabase(int number);
        ISubscriber GetSubscriber();
        void Subscribe(string channel, Action<RedisChannel, RedisValue> action);
        Task<long> Publish(string channel, string message);
        Task<bool> SetKey(Iint stackExchangeRedisDatabase, string key, string value, TimeSpan? expire = null);
        Task<string> GetKey(Iint stackExchangeRedisDatabase, string key);
        Task<string> GetSetKey(Iint stackExchangeRedisDatabase, string key, string value, TimeSpan? expire = null);
        Task<bool> DeleteKey(Iint stackExchangeRedisDatabase, string key);
        Task<long> IncrementKey(Iint stackExchangeRedisDatabase, string key);
        Task<long> IncrementKey(Iint stackExchangeRedisDatabase, string key, long value);
        Task<double> IncrementKey(Iint stackExchangeRedisDatabase, string key, double value);
        Task<string> HGet(Iint stackExchangeRedisDatabase, string hash, string key);
        Task<string> LPop(Iint redisDatabase, string hash);
        Task<long> RPush(Iint redisDatabase, string hash, string value);
        Task<long> LPush(Iint redisDatabase, string hash, string value);
        Task<HashEntry[]> HGetAll(Iint stackExchangeRedisDatabase, string hash);
        Task<bool> HSet(Iint stackExchangeRedisDatabase, string hash, string key, string value);
        Task<double> HIncrementKey(Iint stackExchangeRedisDatabase, string hash, string key, double value);
        Task<TimeSpan?> GetTimeToLive(Iint stackExchangeRedisDatabase, string key);
        List<string> LRange(Iint redisDatabase, string hash, int count);
        Task<bool> Expire(Iint stackExchangeRedisDatabase, string key, TimeSpan expire);
        Task<bool> Move(Iint stackExchangeRedisDatabase, int destinationDatebase, string key);
        List<string> Scan(int database, int pagesize, string pattern = "");
        Task<long> Count(int database);
    }
    public interface Iint
    {
        int Number { get; }
    }
    public class ScanResult
    {
        public List<string> Keys { get; set; }
        public long Cursor { get; set; }
    }
    internal class ConnectionFailureException : Exception
    {
        public ConnectionFailureException() { }

        public ConnectionFailureException(Exception innerException) : base("Connection failed!", innerException) { }
    }
}
