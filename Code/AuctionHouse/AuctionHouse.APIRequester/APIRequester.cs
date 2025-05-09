using System.Net.Http.Json;

namespace AuctionHouse.Requester
{
    public class APIRequester
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        

        public APIRequester(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = Environment.GetEnvironmentVariable("AuctionApiBaseUrl");
        }

        public async Task<string> Get(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string endpoint, object data)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/{endpoint}", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Put(string endpoint, object data)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{endpoint}", data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete(string endpoint)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }




    }
}
