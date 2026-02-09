

using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface I_IdentificacaoAppService
    {
        Task<RetornoGenerico> IdentificacaoUsuario(string email);
    }
}
