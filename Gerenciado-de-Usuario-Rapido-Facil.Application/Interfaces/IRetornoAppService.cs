using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using System.Net;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface IRetornoAppService
    {
        RetornoGenerico CriarRetorno(bool sucesso, HttpStatusCode status, string mensagemSistema, string mensagemUsuario, object dados = null);
    }
}
