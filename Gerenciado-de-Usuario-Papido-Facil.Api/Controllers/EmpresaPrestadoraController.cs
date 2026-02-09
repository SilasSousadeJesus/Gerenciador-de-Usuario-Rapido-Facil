using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Papido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaPrestadoraController : ControllerBase
    {
        private readonly IEmpresaPrestadoraAppService _appService;
        public EmpresaPrestadoraController(IEmpresaPrestadoraAppService empresaPrestadoraAppService)
        {
            _appService = empresaPrestadoraAppService;
        }


        [HttpPost()]
        public async Task<IActionResult> CadastrarEmpresaAsync([FromBody] EmpresaPrestadoraDTO empresa)
        {
            var dados = await _appService.CadastrarEmpresaAsync(empresa);

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

        [HttpGet("BuscarEmpresa/{empresaId}")]
        public async Task<IActionResult> BuscarUmaEmpresa([FromRoute] Guid empresaId)
        {

            var dados = await _appService.BuscarEmpresaAsync(empresaId);

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

        [HttpGet("BuscarTodasEmpresas")]
        public async Task<IActionResult> BuscarTodasEmpresas()
        {

            var dados = await _appService.BuscarTodosAsEmpresasAsync();

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

        [HttpPatch("EditarEmpresa/{empresaId}")]
        public async Task<IActionResult> EditarEmpresaAsync([FromRoute] Guid empresaId, [FromBody] EmpresaPrestadoraAtualizacaoDTO empresaPrestadoraAtualizacaoDTO)
        {

            var dados = await _appService.EditarEmpresaAsync(empresaId, empresaPrestadoraAtualizacaoDTO);

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

        [HttpDelete("DeletarEmpresa/{empresaId}")]
        public async Task<IActionResult> DeletarCondomino([FromRoute] Guid empresaId)
        {

            var dados = await _appService.DeletarEmpresaAsync(empresaId);

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

        [HttpGet("BuscarPrestadorPorFiltros")]
        public async Task<IActionResult> BuscarPrestadorPorFiltros([FromQuery] BuscarPrestadorPorFiltrosDTO filtros)
        {
            var dados = await _appService.BuscarPrestadorPorFiltros(filtros);

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

        [HttpPatch("FinalizarCadastroEmpresaPrestadora/{empresaId}")]
        public async Task<IActionResult> FinalizarCadastroEmpresaPrestadora([FromRoute] Guid empresaId, [FromBody] FinalizarCadastroEmpresaPrestadora finalizarCadastroEmpresaPrestadora)
        {

            var dados = await _appService.FinalizarCadastroEmpresaPrestadora(empresaId, finalizarCadastroEmpresaPrestadora);

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

        [HttpGet("BuscarEmpresaPorEmail/{email}")]
        public async Task<IActionResult> BuscarEmpresaPorEmail([FromRoute] string email)
        {

            var dados = await _appService.BuscarEmpresaPorEmailAsync(email);

            if (dados is null)
            {
                return NotFound();
            }

            return Ok(dados);
        }

        [HttpPatch("VincularEmpresaAServicoSubServico/{empresaId}")]
        public async Task<IActionResult> VincularEmpresaAServicoSubServico([FromRoute] Guid empresaId, [FromBody] List<EmpresaPrestadoraServicoSubtipoDTO> Servicos)
        {
            var dados = await _appService.VincularEmpresaAServicoSubServico(empresaId, Servicos);

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

        [HttpGet("InformacoesPrestador/{empresaId}")]
        public async Task<IActionResult> InformacoesPrestador([FromRoute] Guid empresaId)
        {

            var dados = await _appService.AgruparInformacoesPrestador(empresaId);

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

        [HttpPost("TrocarSenha/{empresaId}")]
        public async Task<IActionResult> TrocarSenha([FromRoute] Guid empresaId, [FromBody] TrocaSenhaDTO trocaSenhaDTO)
        {
            var dados = await _appService.TrocarSenha(empresaId, trocaSenhaDTO);

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


