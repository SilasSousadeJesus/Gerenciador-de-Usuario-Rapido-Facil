using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Usuario_Rapido_Facil.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICondominioAppService _appService;
        public AdminController(ICondominioAppService condominioAppService)
        {
            _appService = condominioAppService;
        }


        //[HttpPost("CriarCondominio")]
        //public async Task<IActionResult> CriarCondominio([FromBody] CadastroCondominioDTO condominioDTO)
        //{
        //    var dados = await _appService.CadastrarCondominioAsync(condominioDTO);

        //    if (!dados.Sucesso)
        //    {
        //        return dados.HttpStatusCode switch
        //        {
        //            System.Net.HttpStatusCode.Unauthorized => Unauthorized(dados),
        //            System.Net.HttpStatusCode.NotFound => NotFound(dados),
        //            System.Net.HttpStatusCode.BadRequest => BadRequest(dados),
        //            System.Net.HttpStatusCode.InternalServerError => StatusCode(500, dados),
        //            _ => BadRequest(dados)
        //        };
        //    }

        //    return Created("criado", dados);
        //}
    }
}
