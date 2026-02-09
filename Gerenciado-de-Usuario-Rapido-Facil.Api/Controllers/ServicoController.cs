using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly IServicoAppService _appService;
        public ServicoController(IServicoAppService servicoAppService)
        {
            _appService = servicoAppService;
        }

        [HttpGet("BuscarTodos")]
        public async Task<IActionResult> BuscarTodos()
        {
            var dados = await _appService.BuscarTodosOsServicosAsync();

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

        [HttpPost("VincularServicoAEmpresa/{empresaPrestadoraId}")]
        public async Task<IActionResult> VincularServicoAEmpresa(Guid empresaPrestadoraId, List<EmpresaPrestadoraServicoSubtipoDTO> empresaPrestadoraServicoSubtipoDTO)
        {
            var dados = await _appService.VincularServicoAEmpresa(empresaPrestadoraId, empresaPrestadoraServicoSubtipoDTO);

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
