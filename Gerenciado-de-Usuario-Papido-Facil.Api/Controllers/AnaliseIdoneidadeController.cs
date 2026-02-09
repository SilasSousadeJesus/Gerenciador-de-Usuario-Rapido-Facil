using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Usuario_Papido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnaliseIdoneidadeController : ControllerBase
    {
        private readonly IAnaliseIdoneidadeAppService _appService;
        public AnaliseIdoneidadeController(IAnaliseIdoneidadeAppService appService)
        {
            _appService = appService;
        }

        [HttpPost("{empresaId}")]
        public async Task<IActionResult> LerCertidoes(Guid empresaId, [FromBody] LerCertidaoDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.PdfBase64))
            {
                return BadRequest(new RetornoGenerico(
                    false,
                    "O PDF em base64 é obrigatório",
                    "O campo PdfBase64 não pode ser vazio",
                    HttpStatusCode.BadRequest
                ));
            }
            var dados = await _appService.LerCertidoes(
                empresaId,
                request.PdfBase64,
                request.EspecieCertidao
            );
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

        [HttpGet("BuscarIdoneidade/{empresaId}")]
        public async Task<IActionResult> BuscarIdoneidade(Guid empresaId)
        {
            var dados = await _appService.BuscarIdoneidade(
                empresaId
            );
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
