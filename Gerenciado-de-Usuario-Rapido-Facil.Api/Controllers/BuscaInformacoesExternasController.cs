using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuscaInformacoesExternasController : ControllerBase
    {
        private readonly IBuscaInformacoesExternas _buscaInformacoesExternas;
        public BuscaInformacoesExternasController(IBuscaInformacoesExternas buscaInformacoesExternas)
        {
            _buscaInformacoesExternas = buscaInformacoesExternas;
        }


        [HttpGet("BuscarTodosEstadosBrasileiros")]
        public async Task<IActionResult> BuscarTodos()
        {
            var dados = await _buscaInformacoesExternas.ObterEstadosAsync();

            if (!dados.Sucesso)
            {
                return dados.HttpStatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => Unauthorized(dados),
                    System.Net.HttpStatusCode.NotFound => NotFound(dados),
                    System.Net.HttpStatusCode.BadRequest => BadRequest(dados),
                    System.Net.HttpStatusCode.InternalServerError => StatusCode(500, dados),
                    _ => BadRequest(dados)
                };
            }

            return Ok(dados);
        }


        [HttpGet("BuscarMunicipiosDeUF/{UF}")]
        public async Task<IActionResult> ObterMunicipiosAsync(string UF)
        {
            var dados = await _buscaInformacoesExternas.ObterMunicipiosAsync(UF);

            if (!dados.Sucesso)
            {
                return dados.HttpStatusCode switch
                {
                    System.Net.HttpStatusCode.Unauthorized => Unauthorized(dados),
                    System.Net.HttpStatusCode.NotFound => NotFound(dados),
                    System.Net.HttpStatusCode.BadRequest => BadRequest(dados),
                    System.Net.HttpStatusCode.InternalServerError => StatusCode(500, dados),
                    _ => BadRequest(dados)
                };
            }

            return Ok(dados);
        }

    }
}
