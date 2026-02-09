using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface INotificacaoAppService
    {


        Task<RetornoGenerico> NotificarUsuarioAsync(Guid userId, string metodo, string message, Guid idRelacionado);

        Task<RetornoGenerico> NotificarUsuariosAsync(List<Guid> userIds, string metodo, string message, ENotificacao eNotificacao, Guid idRelacionado);

        Task<RetornoGenerico> NotificarTodosUsuariosAsync(string message, ETipoUsuario eTipoUsuario, ENotificacao  eNotificacao);

        Task<RetornoGenerico> BuscarTodasNotificacoesAsync(Guid usuarioId);

        Task<RetornoGenerico> BuscarNotificacaoAsync(Guid usuarioId);

        Task<RetornoGenerico> MarcarComoLidoNotificacaoAsync(Guid notificaoId);

        Task<RetornoGenerico> MarcarTodosComoLidoNotificacaoAsync(Guid usuarioId);

        Task<RetornoGenerico> GestaoNotificacao(List<Guid> listaIds, ENotificacao tipoNotificacao, Guid idRelacionado);

        Task<RetornoGenerico> GestaoNotificacao(Guid usuarioId, ENotificacao tipoNotificacao, Guid idRelacionado);
    }
}
