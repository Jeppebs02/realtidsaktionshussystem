using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuctionHouse.Requester
{
    public class APIRequester
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public APIRequester(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = Environment.GetEnvironmentVariable("AuctionApiBaseUrl");

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false
            };
        }

        public async Task<string> Get(string endpoint)
        {
            Console.WriteLine($"Sending GET request to: {_baseUrl}/{endpoint}");
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string endpoint, object data)
        {
            Console.WriteLine($"Sending POST request to: {_baseUrl}/{endpoint}");

            var json = System.Text.Json.JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            Console.WriteLine("Actual JSON payload:");
            Console.WriteLine(json); // ✅ Now you're logging the real body

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ERROR] Status: {response.StatusCode}, Body: {errorBody}");
                throw new HttpRequestException($"HTTP {response.StatusCode}: {errorBody}");
            }

            return await response.Content.ReadAsStringAsync();
        }


        public async Task<string> Put(string endpoint, object data)
        {
            Console.WriteLine($"Sending PUT request to: {_baseUrl}/{endpoint}");

            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}/{endpoint}", content);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete(string endpoint)
        {
            Console.WriteLine($"Sending DELETE request to: {_baseUrl}/{endpoint}");
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
