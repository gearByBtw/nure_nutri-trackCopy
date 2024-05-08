using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Nutritionix
{
    public class NutritionixClient : INutritionixClient
    {
        private readonly string _baseAddress;

        private readonly string _apiKey;

        private readonly string _appId;

        public NutritionixClient(string address, string apiKey, string appId)
        {
            _baseAddress = address;
            _apiKey = apiKey;
            _appId = appId;
        }

        public NutritionixClient(IOptionsSnapshot<NutritionixSettings> settings)
        {
            _baseAddress = settings.Value.BaseAddress;
            _apiKey = settings.Value.ApiKey;
            _appId = settings.Value.AppId;
        }

        public async Task<NutritionData?> GetNutritionData(string query)
        {
            var requestBody = GenerateRequestBody(query);

            using (var client = CreateHttpClient())
            {
                var response = await client.PostAsync("natural/nutrients", requestBody);

                var data = await ConvertResponse<NutritionData>(response);

                return data;
            }
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Add("x-app-id", _appId);
            client.DefaultRequestHeaders.Add("x-app-key", _apiKey);

            return client;
        }

        private HttpContent GenerateRequestBody(string query)
        {
            string requestBody = "{\"query\": \"" + query + "\"}";

            return new StringContent(requestBody, Encoding.UTF8, "application/json");
        }

        private async Task<TResult?> ConvertResponse<TResult>(HttpResponseMessage response)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResult>(jsonResponse);
        }
    }
}
