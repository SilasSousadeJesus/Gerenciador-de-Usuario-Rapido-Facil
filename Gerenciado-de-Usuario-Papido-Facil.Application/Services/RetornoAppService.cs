using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using System.Net;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class RetornoAppService : IRetornoAppService
    {
        public RetornoGenerico CriarRetorno(bool sucesso, HttpStatusCode status, string mensagemSistema, string mensagemUsuario, object dados = null)
        {
            return new RetornoGenerico
            {
                Sucesso = sucesso,
                HttpStatusCode = status,
                MensagemSistema = mensagemSistema,
                MensagemUsuario = mensagemUsuario,
                Dados = dados
            };
        }
    }
}
