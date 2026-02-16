using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MocksController : ControllerBase
    {
        private readonly IEmpresaPrestadoraAppService _empresaPrestadoraService;
        private readonly IServicoAppService _servicoAppServico;
        private readonly ICondominoAppService _condominoAppService;
        private readonly ICondominioAppService _condominioAppService;
        public MocksController(IEmpresaPrestadoraAppService empresaPrestadoraAppService, IServicoAppService servicoAppServico, ICondominoAppService condominoAppService, ICondominioAppService condominioAppService)
        {
            _empresaPrestadoraService = empresaPrestadoraAppService;
            _servicoAppServico = servicoAppServico;
            _condominoAppService = condominoAppService;
            _condominioAppService = condominioAppService;
        }

        [HttpPost("GeracaoDeServicos")]
        public async Task<IActionResult> GeracaoDeServicos()
        {
            var dados = await _servicoAppServico.GeracaoDeServicoServicoSubTipoAsync();

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

        [HttpPost("CadastrarEmpresasDeTeste/{numeroDePrestadores}")]
        public async Task<IActionResult> CadastrarEmpresasDeTeste([FromRoute] int numeroDePrestadores)
        {
            var dados = await _empresaPrestadoraService.CadastrarEmpresasParaTesteAsync(numeroDePrestadores);

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

        [HttpPost("CadastrarCondominosDeTeste/{condominioId}/{numeroDeCondomino}")]
        public async Task<IActionResult> CadastrarCondominoDeTeste([FromRoute] int numeroDeCondomino, [FromRoute] Guid condominioId)
        {
            var dados = await _condominoAppService.CadastrarCondominoParaTesteAsync(condominioId, numeroDeCondomino);

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

        [HttpPost("CadastrarCondominiosDeTeste/{numerosDeCondominios}")]
        public async Task<IActionResult> CadastrarCondominiosDeTeste([FromRoute] int numerosDeCondominios)
        {
            var dados = await _condominioAppService.CadastrarCondominiosParaTesteAsync(numerosDeCondominios);

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
