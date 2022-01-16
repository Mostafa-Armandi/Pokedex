using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Pokedex.Exceptions;

namespace Pokedex.Clients
{
    public class GenericClient<TResult> : IGenericClient<TResult>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        
        public GenericClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<TResult> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new RemoteApiException(response.StatusCode, content);

            return JsonSerializer.Deserialize<TResult>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}