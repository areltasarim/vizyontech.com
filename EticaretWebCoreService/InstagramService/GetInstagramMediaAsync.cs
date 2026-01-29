using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EticaretWebCoreService.InstagramService
{
    public class InstagramService
    {
        private readonly HttpClient _httpClient;

        public InstagramService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetInstagramMediaAsync(string accessToken, string userId)
        {

            var url = $"https://graph.facebook.com/v18.0/{userId}/media?fields=id,caption,media_type,media_url,permalink,timestamp&access_token={accessToken}";
            var response = await _httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content; // JSON string, deserialize as needed
        }
    }
}
