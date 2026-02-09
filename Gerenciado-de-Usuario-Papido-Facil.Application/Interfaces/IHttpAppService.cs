namespace Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces
{
    public interface IHttpAppService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string url, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string url);
        Task<HttpResponseMessage> PatchAsync(string url, HttpContent content);
    }
}
