using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface INotificacaoRepository
    {
        Task<List<Notificacao>> BuscarTodasNotificacoesAsync(Guid usuarioId);
        Task<Notificacao?> BuscarNotificacaoAsync(Guid notificaoId);
        Task EditarNotificacaoAsync(Notificacao notificacao);
        Task EditarTodasNotificacaoAsync(Guid usuarioId);
        Task<bool> CriarNotificacaoAsync(Notificacao notificacao);
        Task<bool> CriarNotificacaoAsync(List<Notificacao> lista);
        Task<bool> NotificarBaseUsuarioAsync(string mensagem, ETipoUsuario eTipoUsuario);
    }
}
