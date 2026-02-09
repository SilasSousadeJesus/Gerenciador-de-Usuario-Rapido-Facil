using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Papido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatAppService _appService;
        public ChatController(IChatAppService chatAppService)
        {
            _appService = chatAppService;
        }


        [HttpPost("CriarChat")]
        public async Task<IActionResult> CriarChat([FromBody] CriarChatDTO criarChatDTO)
        {
            var dados = await _appService.CriarChat(criarChatDTO);

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

        [HttpGet("{chatId}")]
        public async Task<IActionResult> BuscarChat([FromRoute] Guid chatId)
        {
            var dados = await _appService.BuscarChat(chatId);

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

        [HttpGet("BuscarTodasMensagensChat/{chatId}")]
        public async Task<IActionResult> BuscarTodasMensagensChat([FromRoute] Guid chatId)
        {
            var dados = await _appService.BuscarTodasMensagensChat(chatId);

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

        [HttpPost("SalvarMensagem")]
        public async Task<IActionResult> SalvarMensagem([FromBody] CriarMensagemChatDTO mensagemChat)
        {
            var dados = await _appService.SalvarMensagemChat(mensagemChat);

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

            return Created("Criado com sucesso", dados);
        }

    }
}
