
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat?> CriarChat(Chat chat);
        Task<Chat> BuscarChat(Guid chatId);
        Task<Chat?> BuscarPorParticipantes(Guid solicitanteId, Guid EmpresaPrestadoraId, ETipoUsuario eTipoUsuario);
        Task<bool> SalvarMensagemChat(MensagemChat mensagemChat);
        Task<List<MensagemChat>> BuscarTodasMensagensChat(Guid chatId);
    }
}
