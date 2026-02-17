using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICondominioAppService _condominioAppService;
        public AdminController(ICondominioAppService condominioAppService)
        {
            _condominioAppService = condominioAppService;
        }


        [HttpGet("BuscarTodosCondominios")]
        public async Task<IActionResult> BuscarTodosOsCondominio([FromQuery] BuscarCondominioPorFiltrosDTO filtros)
        {

            var dados = await _condominioAppService.BuscarTodosOsCondominioAsync(filtros);

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

        [HttpGet("BuscarUmCondominio/{condominioId}")]
        public async Task<IActionResult> BuscarUmUsuario([FromRoute] Guid condominioId)
        {

            var dados = await _condominioAppService.BuscarUmCondominioAsync(condominioId);

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
