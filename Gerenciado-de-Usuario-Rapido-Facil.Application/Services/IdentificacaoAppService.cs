using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Services
{
    public class IdentificacaoAppService : I_IdentificacaoAppService
    {
        private readonly IConfiguration _configuration;
        private readonly I_IdentificacaoRepository _identificacaoRepository;

        public IdentificacaoAppService(IConfiguration configuration, I_IdentificacaoRepository identificacaoRepository)
        {

            _identificacaoRepository = identificacaoRepository;
            _configuration = configuration;

        }

        public async Task<RetornoGenerico> IdentificacaoUsuario(string email)
        {

            var identificacao = await _identificacaoRepository.IdentificacaoUsuario(email);

            return new RetornoGenerico(!string.IsNullOrWhiteSpace(identificacao) ? true : false, !string.IsNullOrWhiteSpace(identificacao) ? $"Usuario Identificado como {identificacao}" : "Não foi encontrado uma conta com este email", !string.IsNullOrWhiteSpace(identificacao) ? $"Usuario Identificado como {identificacao}" : "Não foi encontrado uma conta com este email", !string.IsNullOrWhiteSpace(identificacao) ? HttpStatusCode.OK : HttpStatusCode.UnprocessableEntity, identificacao);
        }
    }
}
