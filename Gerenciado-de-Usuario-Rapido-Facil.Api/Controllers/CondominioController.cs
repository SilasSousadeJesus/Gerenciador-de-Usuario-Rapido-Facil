using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CondominioController : ControllerBase
    {
        private readonly ICondominioAppService _appService;
        public CondominioController(ICondominioAppService condominioAppService)
        {
            _appService = condominioAppService;
        }


        [HttpPost("CriarCondominio")]
        public async Task<IActionResult> CriarCondominio([FromBody] CadastroCondominioDTO condominioDTO)
        {
            var dados = await _appService.CadastrarCondominioAsync(condominioDTO);

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

            return Created("criado", dados);
        }

        [HttpGet("{condominioId}")]
        public async Task<IActionResult> BuscarUmUsuario([FromRoute] Guid condominioId)
        {

            var dados = await _appService.BuscarUmCondominioAsync(condominioId);

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

        [HttpPatch("AtualizarCondominio/{condominioId}")]
        public async Task<IActionResult> EditarCondominio([FromRoute] Guid condominioId, [FromBody] EdicaoCondominioDTO edicaoCondominioDTO)
        {

            var dados = await _appService.EditarCondominioAsync(condominioId, edicaoCondominioDTO);

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

        [HttpDelete("DeletarCondominio/{condominioId}")]
        public async Task<IActionResult> DeletarCondominio([FromRoute] Guid condominioId)
        {

            var dados = await _appService.DeletarCondominioAsync(condominioId);

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

        [HttpGet("BuscarPorCodigoVinculacao/{codigoVinculacao}")]
        public async Task<IActionResult> BuscarCondominioPorCodigoVinculacao([FromRoute] string codigoVinculacao)
        {

            var dados = await _appService.BuscarUmCondominioPorCodigoVinculacaoAsync(codigoVinculacao);

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

        [HttpGet("BuscarUmCondominioPorEmail/{email}")]
        public async Task<IActionResult> BuscarUmUsuarioPorEmail([FromRoute] string email)
        {
            var dados = await _appService.BuscarUmCondominioPorEmailAsync(email);

            if (dados is null)
            {
                return NotFound(dados);
            }

            return Ok(dados);
        }

        [HttpPost("FinalizarPeriodoTeste/{condominioId}")]
        public async Task<IActionResult> FinalizarPeriodoTeste([FromRoute] Guid condominioId)
        {
            var dados = await _appService.FinalizarPeriodoTeste(condominioId);

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

        [HttpPost("EnviarEmailVinculo/{destinatario}")]
        public async Task<IActionResult> EnviarEmailVinculo([FromRoute] string destinatario)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { mensagem = "Token não encontrado" });
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var usuarioIdString = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (!Guid.TryParse(usuarioIdString, out var usuarioIdRemetente))
                {
                    return BadRequest(new { mensagem = "ID de usuário inválido no token" });
                }

                var dados = await _appService.EnviarEmailVincularCondomino(destinatario, usuarioIdRemetente);

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
                return Created("criado", dados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Token inválido", erro = ex.Message });
            }
        }

        [HttpPost("TrocarSenha/{condominioId}")]
        public async Task<IActionResult> TrocarSenha([FromRoute] Guid condominioId, [FromBody] TrocaSenhaDTO trocaSenhaDTO)
        {
            var dados = await _appService.TrocarSenha(condominioId, trocaSenhaDTO);

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

            return Created("criado", dados);
        }

    }
}
