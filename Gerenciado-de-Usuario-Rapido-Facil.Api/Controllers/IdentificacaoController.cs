using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificacaoController : ControllerBase
    {

        private readonly I_IdentificacaoAppService _identificacaoAppService;
        public IdentificacaoController(I_IdentificacaoAppService identificacaoAppService)
        {
            _identificacaoAppService = identificacaoAppService;
        }
        [HttpGet("IdentificacaoPorEmail/{email}")]
        public async Task<IActionResult> IdentifcacaoUsuario([FromRoute] string email)
        {
            var dados = await _identificacaoAppService.IdentificacaoUsuario(email);

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
