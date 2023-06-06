using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Insurance.Infrastructure.Services.ApiClient
{
    public class ApiClient: IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClient> _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string apiUrl)
        {
            if (string.IsNullOrWhiteSpace(apiUrl))
                throw new ArgumentNullException(nameof(apiUrl));

            T result = default;

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, apiUrl))
                {
                    var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    await response.Content.ReadAsStringAsync().ContinueWith(resp =>
                    {
                        if (resp.IsFaulted && resp.Exception != null) throw resp.Exception;

                        result = JsonConvert.DeserializeObject<T>(resp.Result);
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot Connect or call Api {ex.Message}\n{ex.StackTrace}");
            }

            return result;
        }
    }
}
