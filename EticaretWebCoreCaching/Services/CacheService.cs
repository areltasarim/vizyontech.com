using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using EticaretWebCoreCaching.Abstraction;

namespace EticaretWebCoreCaching.Services
{
    public class CacheService : ICacheService
    {
        public string KeyPrefix { get; set; }
        private StackExchange.Redis.IDatabase _db;
        private StackExchange.Redis.ConnectionMultiplexer _Connection;
        private readonly IConfiguration _configuration;
        private Lazy<ConnectionMultiplexer> lazyConnection;

        public CacheService(IConfiguration configuration)
        {
            _configuration = configuration;
            ConfigureRedis(_configuration);
        }

        private void ConfigureRedis(IConfiguration configuration)
        {
            KeyPrefix = configuration.GetSection("CahceSettings:KeyPrefix").Value.ToString();
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string redisConnectionString = configuration.GetSection("CahceSettings:ConnectionString").Value.ToString();
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });
            _Connection = lazyConnection.Value;
            _db = _Connection.GetDatabase();
        }



        #region Utilities

        private byte[] SerializeAndCompress(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            using (GZipStream zs = new GZipStream(ms, CompressionMode.Compress, true))
            {
                BinaryFormatter bf = new BinaryFormatter();
#pragma warning disable SYSLIB0011
                bf.Serialize(zs, obj);
#pragma warning restore SYSLIB0011
                return ms.ToArray();
            }
        }

        private T DecompressAndDeserialize<T>(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            using (GZipStream zs = new GZipStream(ms, CompressionMode.Decompress, true))
            {
                BinaryFormatter bf = new BinaryFormatter();
#pragma warning disable SYSLIB0011
                return (T)bf.Deserialize(zs);
#pragma warning restore SYSLIB0011

            }
        }

        private string Compress(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());
            }
        }

        private string Decompress(string value)
        {
            var bytes = Convert.FromBase64String(value);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        protected byte[] Serialize(object item)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //ContractResolver = ShouldSerializeContractResolver.Instance
            };
            var jsonString = JsonConvert.SerializeObject(item, Formatting.Indented, serializerSettings);
            //jsonString = Compress(jsonString);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        protected T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
                return default(T);

            var jsonString = Encoding.UTF8.GetString(serializedObject);
            //jsonString = Decompress(jsonString);
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //ContractResolver = ShouldSerializeContractResolver.Instance
            };
            return JsonConvert.DeserializeObject<T>(jsonString, serializerSettings);
        }

        #endregion Utilities

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }
            var rValue = _db.StringGet(key);
            if (!rValue.HasValue)
                return default(T);

            var result = Deserialize<T>(rValue);

            //_perRequestCacheManager.Set(key, result, 0);
            return result;
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime = 30, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }
            if (data == null)
                return;

            //var entryBytes = Serialize(data);
            var entryBytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data);
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            _db.StringSet(key, entryBytes, expiresIn);
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetAsync<T>(string key, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }
            //little performance workaround here:
            //we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)

            //if (_perRequestCacheManager.IsSet(key))
            //    return _perRequestCacheManager.Get<T>(key);

            //get serialized item from cache
            var serializedItem = await _db.StringGetAsync(key);
            
            if (!serializedItem.HasValue)
                return default(T);

            //deserialize item
            //var item = Deserialize<T>(serializedItem);

            var item = System.Text.Json.JsonSerializer.Deserialize<T>(serializedItem);
            if (item == null)
                return default(T);

            //set item in the per-request cache
            //_perRequestCacheManager.Set(key, item, 0);

            return item;
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        public async Task SetAsync(string key, object data, int cacheTime, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }

            if (data == null)
                return;

            // Cache süresi ayarla
            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            // Serialize işlemi - circular referanslardan korur
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var jsonString = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            var serializedItem = Encoding.UTF8.GetBytes(jsonString);

            // Redis'e yaz
            await _db.StringSetAsync(key, serializedItem, expiresIn);
        }


        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }
            //little performance workaround here:
            //we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server 500 times per HTTP request (e.g. each time to load a locale or setting)

            //if (_perRequestCacheManager.IsSet(key))
            //    return true;

            return _db.KeyExists(key);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        public async Task<bool> IsSetAsync(string key, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }
            //little performance workaround here:
            //we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)

            //if (_perRequestCacheManager.IsSet(key))
            //    return true;

            return await _db.KeyExistsAsync(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key, bool isKeyPrefix = true)
        {
            if (isKeyPrefix)
            {
                key = KeyPrefix + key;
            }
            _db.KeyDelete(key);
            //_perRequestCacheManager.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            foreach (var server in _Connection.GetServers())
            {
                var keys = server.Keys(database: _db.Database, pattern: "*" + pattern + "*").ToList();
                foreach (var key in keys)
                {
                    Remove(key, false);
                }

            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var server in _Connection.GetServers())
            {
                var keys = server.Keys(database: _db.Database);
                foreach (var key in keys)
                    Remove(key);
            }
        }
    }

}