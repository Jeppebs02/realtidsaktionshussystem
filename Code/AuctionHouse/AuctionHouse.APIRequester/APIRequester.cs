using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

namespace AuctionHouse.Requester
{
    public class APIRequester
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _serializerOptions;
        

        public APIRequester(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = Environment.GetEnvironmentVariable("AuctionApiBaseUrl");
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };  
        }

        public async Task<string> Get(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string endpoint, object data)
        {
            // Serialize the Object
            var json = System.Text.Json.JsonSerializer.Serialize(data, _serializerOptions);
            Console.WriteLine(json); //Just debugging

            // Add header and define encodeing type
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine(content); //Just debugging

            // Send the JSON object
            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);

            // Return the response
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
