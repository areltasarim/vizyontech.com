using DocumentFormat.OpenXml.Spreadsheet;
using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dimension = Google.Analytics.Data.V1Beta.Dimension;

namespace EticaretWebCoreHelper
{
    public class AnalyticsService
    {
        private readonly IConfiguration _configuration;

        public AnalyticsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      
        public async Task<int> GetRealtimeActiveUsersAsync()
        {
            var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
            var propertyId = _configuration["GoogleAnalytics:PropertyId"];

            var credential = GoogleCredential
                .FromFile(credentialsPath)
                .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

            var accessToken = await credential.UnderlyingCredential
                .GetAccessTokenForRequestAsync();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runRealtimeReport";

            var body = new
            {
                metrics = new[] {
            new { name = "activeUsers" }
        }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseContent);

            if (jsonDoc.RootElement.TryGetProperty("rows", out var rows))
            {
                var value = rows[0]
                    .GetProperty("metricValues")[0]
                    .GetProperty("value")
                    .GetString();

                return int.Parse(value);
            }
            else
            {
                return 0; // veya hata mesajı
            }
        }
        public class CountryUser
        {
            public string Country { get; set; }
            public int Users { get; set; }
        }
    
        public async Task<Dictionary<string, int>> GetRealtimeUsersByCityAsync()
        {
            var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
            var propertyId = _configuration["GoogleAnalytics:PropertyId"];

            var credential = GoogleCredential
                .FromFile(credentialsPath)
                .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var client = new HttpClient();
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

            // 🔍 1. HTTP başarısız mı?
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ HTTP Hatası: {response.StatusCode}");
                Console.WriteLine($"Yanıt: {responseContent}");
                return new Dictionary<string, int>();
            }

            var jsonDoc = JsonDocument.Parse(responseContent);

            // 🔍 2. API hata mesajı var mı?
            if (jsonDoc.RootElement.TryGetProperty("error", out var error))
            {
                return new Dictionary<string, int>();
            }

            var result = new Dictionary<string, int>();

            // 🔍 3. Gelen veriyi oku
            if (jsonDoc.RootElement.TryGetProperty("rows", out var rows))
            {
                foreach (var row in rows.EnumerateArray())
                {
                    var city = row.GetProperty("dimensionValues")[0].GetProperty("value").GetString();
                    var value = int.Parse(row.GetProperty("metricValues")[0].GetProperty("value").GetString());

                    if (!string.IsNullOrEmpty(city))
                        result[city] = value;
                }
            }
            else
            {
                Console.WriteLine("⚠️ Veri geldi ama rows yok.");
            }


            return result;
        }


        public async Task<Dictionary<string, int>> GetTopPagesAsync()
        {
            var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
            var propertyId = _configuration["GoogleAnalytics:PropertyId"];

            var credential = GoogleCredential
                .FromFile(credentialsPath)
                .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runReport";

            var body = new
            {
                dateRanges = new[]
                {
            new { startDate = "today", endDate = "today" } // Bugünkü veriler
        },
                dimensions = new[]
                {
            new { name = "pageTitle" } // veya "pagePath" kullanabilirsin
        },
                metrics = new[]
                {
            new { name = "screenPageViews" } // sayfa görüntüleme sayısı
        },
                limit = 10
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("✅ HTTP Status Code: " + response.StatusCode);


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
            }
            else
            {
                Console.WriteLine("⚠️ 'rows' bulunamadı veya boş.");
            }

            return result;
        }


        public async Task<Dictionary<string, int>> GetRealtimeUsersByDeviceAsync()
        {
            var credentialsPath = _configuration["GoogleAnalytics:CredentialsFile"];
            var propertyId = _configuration["GoogleAnalytics:PropertyId"];

            var credential = GoogleCredential
                .FromFile(credentialsPath)
                .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://analyticsdata.googleapis.com/v1beta/properties/{propertyId}:runRealtimeReport";

            var body = new
            {
                dimensions = new[] {
            new { name = "deviceCategory" } // ✅ Burada cihaz türü alıyoruz
        },
                metrics = new[] {
            new { name = "activeUsers" }
        }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();


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
            }

            return result;
        }




 

    }

}
