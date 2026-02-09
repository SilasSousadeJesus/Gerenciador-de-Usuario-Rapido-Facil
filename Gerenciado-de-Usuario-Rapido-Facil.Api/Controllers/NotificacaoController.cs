using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoAppService _appService;
        public NotificacaoController(INotificacaoAppService appService)
        {
            _appService = appService;
        }

        [HttpGet("BuscarTodasNotificacoes/{usuarioId}")]
        public async Task<IActionResult> BuscarUmaCotacaoCondominio(Guid usuarioId)
        {
            var dados = await _appService.BuscarTodasNotificacoesAsync(usuarioId);

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

        [HttpGet("BuscarNotificacao/{notificaoId}")]
        public async Task<IActionResult> BuscarNotificacao(Guid notificaoId)
        {
            var dados = await _appService.BuscarNotificacaoAsync(notificaoId);

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

        [HttpPut("MarcarComoLido/{notificaoId}")]
        public async Task<IActionResult> MarcarComoLido(Guid notificaoId)
        {
            var dados = await _appService.MarcarComoLidoNotificacaoAsync(notificaoId);

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

        [HttpPut("MarcarTodosComoLido/{usuarioId}")]
        public async Task<IActionResult> MarcarTodosComoLido(Guid usuarioId)
        {
            var dados = await _appService.MarcarTodosComoLidoNotificacaoAsync(usuarioId);

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

        [HttpPost("NotificarUsuario/{usuarioId}")]
        public async Task<IActionResult> NotificarUsuario([FromRoute]Guid usuarioId, [FromBody] NotificarUmUsuarioDTO DTO)
        {
            var dados = await _appService.GestaoNotificacao(usuarioId, DTO.tipoNotificacao, DTO.idRelacionado);

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

        [HttpPost("NotificarVariosUsuarios")]
        public async Task<IActionResult> NotificarVariosUsuarios([FromBody] NotificarVariosUsuariosDTOs DTO)
        {
            var dados = await _appService.GestaoNotificacao(DTO.userIds, DTO.tipoNotificacao, DTO.idRelacionado);

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
