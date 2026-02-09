using AutoMapper;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Services
{
    public class ValidacaoUsuarioAppService : IValidacaoUsuarioAppService
    {
        private readonly IMapper _mapper;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IValidacaoParaCadastroRepository _validacaoParaCadastroUsuario;
        private readonly IConfiguration _configuration;
        private readonly IHttpAppService _httpAppService;
        public ValidacaoUsuarioAppService(
                ICondominioRepository condominioRepository,
                IMapper mapper,
                IValidacaoParaCadastroRepository validacaoParaCadastroRepository,
                IConfiguration configuration,
                IHttpAppService httpAppService
            )
        {
            _mapper = mapper;
            _condominioRepository = condominioRepository;
            _validacaoParaCadastroUsuario = validacaoParaCadastroRepository;
            _configuration = configuration;
            _httpAppService = httpAppService;
        }

        public async Task<RetornoGenerico> ObterNumeroDeServicosRecebidosAsync(Guid usuarioId)
        {

            var ulr = _configuration.GetSection("CotacaoApi")["ContarServicosRecebidos"];

            if (string.IsNullOrEmpty(ulr)) return new RetornoGenerico(false,
                                             "URL da api de cotação não fornecedia",
                                            "URL da api de cotação não fornecedia",
                                             HttpStatusCode.BadGateway
                                         );

            var response = await _httpAppService.GetAsync(ulr.Replace("{usuarioId}", usuarioId.ToString()));

            if (response is null || !response.IsSuccessStatusCode)
            {
                return new RetornoGenerico(false,
                "Não foi possivel enviar o id do condominio para a api de cotação",
                "",
                HttpStatusCode.BadRequest,
                null
               );
            }

            return new RetornoGenerico(true, "lista de ordem de serviço  encontrada", HttpStatusCode.OK, response.Content);

            throw new NotImplementedException();
        }
    }
}
