using DocumentFormat.OpenXml.Spreadsheet;
using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using EticaretWebCoreCaching.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dimension = Google.Analytics.Data.V1Beta.Dimension;

namespace EticaretWebCoreHelper
{
    public class AnalyticsService
    {
        private readonly IConfiguration _configuration;
        private readonly string _logPath;
        private readonly ICacheService _cacheService;
        
        // Rate limiting: Aynı anda maksimum 1 istek
        private readonly SemaphoreSlim _rateLimiter = new SemaphoreSlim(1, 1);
        
        // Cache timeout: 5 dakika (saniye cinsinden)
        private readonly int _cacheDurationSeconds = 300; // 5 dakika = 300 saniye
        
        // API istek timeout'u: 10 saniye
        private readonly TimeSpan _httpTimeout = TimeSpan.FromSeconds(10);

        public AnalyticsService(IConfiguration configuration, ICacheService cacheService)
        {
            _configuration = configuration;
            _cacheService = cacheService;
            _logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "analytics.log");
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath));
        }

        private void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}\n");
            }
            catch { }
        }

        private async Task<T> GetOrSetCacheAsync<T>(string cacheKey, Func<Task<T>> factory)
        {
            // Önce cache'den kontrol et
            var cachedValue = _cacheService.Get<T>(cacheKey);
            
            // Dictionary için boş check, int için 0'dan farklı check
            bool hasValue = false;
            if (typeof(T) == typeof(int))
            {
                hasValue = cachedValue != null && (int)(object)cachedValue > 0;
            }
            else if (typeof(T) == typeof(Dictionary<string, int>))
            {
                hasValue = cachedValue != null && ((Dictionary<string, int>)(object)cachedValue).Count > 0;
            }
            else
            {
                hasValue = cachedValue != null;
            }

            if (hasValue)
            {
                LogToFile($"✅ Cache HIT: {cacheKey}");
                return cachedValue;
            }

            LogToFile($"⚠️ Cache MISS: {cacheKey} - API'den veri çekiliyor...");

            // Cache miss olunca factory çalıştır
            var value = await factory();
            
            // Sadece anlamlı değerleri cache'e yaz
            if (value != null)
            {
                bool shouldCache = false;
                if (typeof(T) == typeof(int))
                {
                    shouldCache = (int)(object)value > 0;
                }
                else if (typeof(T) == typeof(Dictionary<string, int>))
                {
                    shouldCache = ((Dictionary<string, int>)(object)value).Count > 0;
                }
                else
                {
                    shouldCache = true;
                }

                if (shouldCache)
                {
                    _cacheService.Set(cacheKey, value, _cacheDurationSeconds);
                    LogToFile($"✅ Cache SET: {cacheKey}");
                }
                else
                {
                    LogToFile($"⚠️ Boş veri, cache'e yazılmadı: {cacheKey}");
                }
            }

            return value;
        }

        // ===== ACTIVE USERS =====
        public async Task<int> GetRealtimeActiveUsersAsync()
        {
            return await GetOrSetCacheAsync("analytics:active_users", async () =>
            {
                await _rateLimiter.WaitAsync();
                try
                {
                    return await FetchRealtimeActiveUsersAsync();
                }
                finally
                {
                    _rateLimiter.Release();
                }
            });
        }

        private async Task<int> FetchRealtimeActiveUsersAsync()
        {
            try
            {
                var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
                var propertyId = _configuration["GoogleAnalytics:PropertyId"];

                if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
                {
                    LogToFile($"❌ Credentials dosyası bulunamadı: {credentialsPath}");
                    return 0;
                }

                var credential = GoogleCredential
                    .FromFile(credentialsPath)
                    .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                using (var client = new HttpClient() { Timeout = _httpTimeout })
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runRealtimeReport";

                    var body = new { metrics = new[] { new { name = "activeUsers" } } };
                    var json = JsonSerializer.Serialize(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        LogToFile($"❌ API Hatası: {response.StatusCode}");
                        return 0;
                    }

                    var jsonDoc = JsonDocument.Parse(responseContent);
                    if (jsonDoc.RootElement.TryGetProperty("rows", out var rows))
                    {
                        var value = rows[0].GetProperty("metricValues")[0].GetProperty("value").GetString();
                        LogToFile($"✅ Aktif Kullanıcı: {value}");
                        return int.Parse(value);
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogToFile($"❌ FetchRealtimeActiveUsersAsync Hatası: {ex.Message}");
                return 0;
            }
        }

        // ===== USERS BY CITY =====
        public async Task<Dictionary<string, int>> GetRealtimeUsersByCityAsync()
        {
            await _rateLimiter.WaitAsync();
            try
            {
                return await GetOrSetCacheAsync("analytics:users_by_city", async () =>
                {
                    return await FetchRealtimeUsersByCityAsync();
                });
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        private async Task<Dictionary<string, int>> FetchRealtimeUsersByCityAsync()
        {
            try
            {
                var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
                var propertyId = _configuration["GoogleAnalytics:PropertyId"];

                if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
                {
                    LogToFile($"❌ Credentials dosyası bulunamadı");
                    return new Dictionary<string, int>();
                }

                var credential = GoogleCredential
                    .FromFile(credentialsPath)
                    .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                using (var client = new HttpClient() { Timeout = _httpTimeout })
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runRealtimeReport";

                    var body = new
                    {
                        dimensions = new[] { new { name = "city" } },
                        metrics = new[] { new { name = "activeUsers" } }
                    };

                    var json = JsonSerializer.Serialize(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        LogToFile($"❌ API Hatası: {response.StatusCode}");
                        return new Dictionary<string, int>();
                    }

                    var result = new Dictionary<string, int>();
                    var jsonDoc = JsonDocument.Parse(responseContent);

                    if (jsonDoc.RootElement.TryGetProperty("rows", out var rows))
                    {
                        foreach (var row in rows.EnumerateArray())
                        {
                            var city = row.GetProperty("dimensionValues")[0].GetProperty("value").GetString();
                            var value = int.Parse(row.GetProperty("metricValues")[0].GetProperty("value").GetString());

                            if (!string.IsNullOrEmpty(city))
                                result[city] = value;
                        }
                        LogToFile($"✅ {result.Count} şehirden veri alındı");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogToFile($"❌ FetchRealtimeUsersByCityAsync Hatası: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        // ===== TOP PAGES =====
        public async Task<Dictionary<string, int>> GetTopPagesAsync()
        {
            await _rateLimiter.WaitAsync();
            try
            {
                return await GetOrSetCacheAsync("analytics:top_pages", async () =>
                {
                    return await FetchTopPagesAsync();
                });
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        private async Task<Dictionary<string, int>> FetchTopPagesAsync()
        {
            try
            {
                var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
                var propertyId = _configuration["GoogleAnalytics:PropertyId"];

                if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
                {
                    LogToFile($"❌ Credentials dosyası bulunamadı");
                    return new Dictionary<string, int>();
                }

                var credential = GoogleCredential
                    .FromFile(credentialsPath)
                    .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                using (var client = new HttpClient() { Timeout = _httpTimeout })
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runReport";

                    var body = new
                    {
                        dateRanges = new[] { new { startDate = "today", endDate = "today" } },
                        dimensions = new[] { new { name = "pageTitle" } },
                        metrics = new[] { new { name = "screenPageViews" } },
                        limit = 10
                    };

                    var json = JsonSerializer.Serialize(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        LogToFile($"❌ API Hatası: {response.StatusCode}");
                        return new Dictionary<string, int>();
                    }

                    var result = new Dictionary<string, int>();
                    var jsonDoc = JsonDocument.Parse(responseContent);

                    if (jsonDoc.RootElement.TryGetProperty("rows", out var rows))
                    {
                        foreach (var row in rows.EnumerateArray())
                        {
                            var title = row.GetProperty("dimensionValues")[0].GetProperty("value").GetString();
                            var views = int.Parse(row.GetProperty("metricValues")[0].GetProperty("value").GetString());
                            result[title ?? ""] = views;
                        }
                        LogToFile($"✅ {result.Count} sayfa verisi alındı");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogToFile($"❌ FetchTopPagesAsync Hatası: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        // ===== USERS BY DEVICE =====
        public async Task<Dictionary<string, int>> GetRealtimeUsersByDeviceAsync()
        {
            await _rateLimiter.WaitAsync();
            try
            {
                return await GetOrSetCacheAsync("analytics:users_by_device", async () =>
                {
                    return await FetchRealtimeUsersByDeviceAsync();
                });
            }
            finally
            {
                _rateLimiter.Release();
            }
        }

        private async Task<Dictionary<string, int>> FetchRealtimeUsersByDeviceAsync()
        {
            try
            {
                var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
                var propertyId = _configuration["GoogleAnalytics:PropertyId"];

                if (string.IsNullOrEmpty(credentialsPath) || !File.Exists(credentialsPath))
                {
                    LogToFile($"❌ Credentials dosyası bulunamadı");
                    return new Dictionary<string, int>();
                }

                var credential = GoogleCredential
                    .FromFile(credentialsPath)
                    .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

                var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

                using (var client = new HttpClient() { Timeout = _httpTimeout })
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runRealtimeReport";

                    var body = new
                    {
                        dimensions = new[] { new { name = "deviceCategory" } },
                        metrics = new[] { new { name = "activeUsers" } }
                    };

                    var json = JsonSerializer.Serialize(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        LogToFile($"❌ API Hatası: {response.StatusCode}");
                        return new Dictionary<string, int>();
                    }

                    var result = new Dictionary<string, int>();
                    var jsonDoc = JsonDocument.Parse(responseContent);

                    if (jsonDoc.RootElement.TryGetProperty("rows", out var rows))
                    {
                        foreach (var row in rows.EnumerateArray())
                        {
                            var device = row.GetProperty("dimensionValues")[0].GetProperty("value").GetString() ?? "Bilinmiyor";
                            var value = int.Parse(row.GetProperty("metricValues")[0].GetProperty("value").GetString());
                            result[device] = value;
                        }
                        LogToFile($"✅ {result.Count} cihaz türünden veri alındı");
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogToFile($"❌ FetchRealtimeUsersByDeviceAsync Hatası: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }

        public class CountryUser
        {
            public string Country { get; set; }
            public int Users { get; set; }
        }
    }
}
