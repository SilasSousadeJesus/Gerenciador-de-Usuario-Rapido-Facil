using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CondominoController : ControllerBase
    {
        private readonly ICondominoAppService _appService;
        public CondominoController(ICondominoAppService condominoAppService)
        {
            _appService = condominoAppService;
        }


        [HttpPost()]
        public async Task<IActionResult> CriarCondomino([FromBody] CadastroCondominoDTO condominoDTO)
        {
            var dados = await _appService.CadastrarCondominoAsync(condominoDTO);

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

        [HttpGet("{condominoId}")]
        public async Task<IActionResult> BuscarUmCondomino([FromRoute] Guid condominoId)
        {

            var dados = await _appService.BuscarUmCondominoAsync(condominoId);

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

        [HttpGet("BuscarTodos/{condominioId}")]
        public async Task<IActionResult> BuscarTodosOsCondominos([FromRoute] Guid condominioId, [FromQuery] BuscarCondominoPorFiltrosDTO filtros)
        {

            var dados = await _appService.BuscarTodosOsCondominosAsync(condominioId, filtros);

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

        [HttpPatch("{condominoId}")]
        public async Task<IActionResult> EditarCondomino([FromRoute] Guid condominoId, [FromBody] EdicaoCondominoDTO edicaoCondominioDTO)
        {

            var dados = await _appService.EditarCondominoAsync(condominoId, edicaoCondominioDTO);

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

        [HttpDelete("{condominoId}")]
        public async Task<IActionResult> DeletarCondomino([FromRoute] Guid condominoId)
        {

            var dados = await _appService.DeletarCondominoAsync(condominoId);

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

        [HttpPost("VincularACondominio/{codigo}/{condominoId}")]
        public async Task<IActionResult> VincularACondominio([FromRoute] string codigo, [FromRoute] Guid condominoId)
        {
            var dados = await _appService.VincularCondominoACondominioAsync(codigo, condominoId);

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

        [HttpGet("BuscarUmCondominoPorEmail/{email}")]
        public async Task<IActionResult> BuscarUmCondominoPorEmail([FromRoute] string email)
        {

            var dados = await _appService.BuscarUmCondominoPorEmailAsync(email);

            if (dados is null)
            {
                return NotFound();
            }

            return Ok(dados);
        }

        [HttpPost("TrocarSenha/{condominoId}")]
        public async Task<IActionResult> TrocarSenha([FromRoute] Guid condominoId, [FromBody] TrocaSenhaDTO trocaSenhaDTO)
        {
            var dados = await _appService.TrocarSenha(condominoId, trocaSenhaDTO);

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

            return Ok();
        }
    }
}
