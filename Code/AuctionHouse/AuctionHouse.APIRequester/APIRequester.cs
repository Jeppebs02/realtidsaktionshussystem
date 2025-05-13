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
            Console.WriteLine($"Sending GET request to: {_baseUrl}/{endpoint}"); // Just logging
            // Send a Get request for a JSON Response
            var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");

            // Exception if unsuccesfull
            response.EnsureSuccessStatusCode();

            // Return the response object
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Post(string endpoint, object data)
        {
            try
            {
                Console.WriteLine($"Sending Post request to: {_baseUrl}/{endpoint}"); // Just logging

                // Serialize the Object
                var json = System.Text.Json.JsonSerializer.Serialize(data, _serializerOptions);
                Console.WriteLine(json); //Just debugging

                // Add header and define encodeing type
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content); //Just debugging

                // Send the JSON object
                var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);

                Console.WriteLine($"POST response status code: {(int)response.StatusCode} {response.ReasonPhrase}");



                // Return the response
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("POST response body: " + result);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP POST failed: {ex.Message}");
                return $"EXCEPTION: {ex.Message}";
            }
        }

        public async Task<string> Put(string endpoint, object data)
        {
            Console.WriteLine($"Sending PUT request to: {_baseUrl}/{endpoint}"); // Just logging

            // Serialize the Object
            var json = System.Text.Json.JsonSerializer.Serialize(data, _serializerOptions);
            Console.WriteLine(json); //Just debugging

            // Add header and define encodeing type
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine(content); //Just debugging

            // Send the JSON object
            var response = await _httpClient.PutAsync($"{_baseUrl}/{endpoint}", content);

            // Return the response
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delete(string endpoint)
        {
            Console.WriteLine($"Sending PUT request to: {_baseUrl}/{endpoint}"); // Just logging

            // Send a Delete request
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/{endpoint}");

            // Exception if unsuccesfull
            response.EnsureSuccessStatusCode();

            // Return the response
            return await response.Content.ReadAsStringAsync();
        }




    }
}
