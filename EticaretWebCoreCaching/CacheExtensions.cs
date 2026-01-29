using EticaretWebCoreCaching.Abstraction;

namespace EticaretWebCoreCaching
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static T Get<T>(this ICacheService cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, CachingDefaults.CacheTime, acquire);
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static async Task<T> GetAsync<T>(this ICacheService cacheManager, string key, Func<Task<T>> acquire, int cacheTime = 0)
        {
            return await GetAsync(cacheManager, key, (cacheTime == 0 ? CachingDefaults.CacheTime : cacheTime), acquire);
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="key">Cache key</param>
        /// <param name="cacheTime">Cache time in minutes (0 - do not cache)</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>Cached item</returns>
        public static T Get<T>(this ICacheService cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();
                if (result == null)
                    return result;


                if (cacheTime > 0)
                    cacheManager.Set(key, result, cacheTime);
                return result;
            }
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        public static async Task<T> GetAsync<T>(this ICacheService cacheManager, string key, int cacheTime, Func<Task<T>> acquire)
        {
            //if (!cacheManager.GetEnabled)
            //{
            //    var result = acquire();
            //    return await result;
            //}

            if (await cacheManager.IsSetAsync(key))
            {
                return await cacheManager.GetAsync<T>(key);
            }
            else
            {
                var result = acquire();
                if (result == null)
                    return await result;
                if (cacheTime > 0)
                    await cacheManager.SetAsync(key, Task.FromResult(result.Result).Result, cacheTime);
                return await result;
            }
        }
    }
}
