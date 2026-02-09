using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Services
{
    public class BuscaInformacoesExternas : IBuscaInformacoesExternas
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<RetornoGenerico> ObterEstadosAsync()
        {
            string url = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";

            var retorno = new RetornoGenerico();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var estados = JsonSerializer.Deserialize<List<Estado>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new RetornoGenerico(true, "Lista de Estados Encontrada", "Lista de Estados Encontrada",  HttpStatusCode.OK, estados.OrderBy(x =>x.Sigla));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return new RetornoGenerico(false, $"Request error: {e.Message}", $"Request error: {e.Message}", HttpStatusCode.InternalServerError);
            }
            catch (JsonException e)
            {
                Console.WriteLine($"JSON error: {e.Message}");
                return new RetornoGenerico(false, $"Request error: {e.Message}", $"Request error: {e.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<RetornoGenerico> ObterMunicipiosAsync(string estadoSigla)
        {
            string url = $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{estadoSigla.ToUpper()}/municipios";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                var municipios = JsonSerializer.Deserialize<List<Municipio>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return new RetornoGenerico(true, "Lista de cidades Encontrada", "Lista de cidades Encontrada", HttpStatusCode.OK, municipios.OrderBy(x => x.Nome));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return new RetornoGenerico(false, $"Request error: {e.Message}", $"Request error: {e.Message}", HttpStatusCode.InternalServerError);
            }
            catch (JsonException e)
            {
                Console.WriteLine($"JSON error: {e.Message}");
                return new RetornoGenerico(false, $"Request error: {e.Message}", $"Request error: {e.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
