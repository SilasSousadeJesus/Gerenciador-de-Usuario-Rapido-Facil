using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class HttpAppService : IHttpAppService
    {
        private readonly HttpClient _httpClient;

        public HttpAppService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return await _httpClient.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, HttpContent content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri(url),
                Content = content
            };
            return await _httpClient.SendAsync(request);
        }
    }
}
