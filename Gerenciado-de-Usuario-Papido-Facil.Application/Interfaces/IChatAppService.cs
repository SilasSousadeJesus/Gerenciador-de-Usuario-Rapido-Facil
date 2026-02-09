using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces
{
    public interface IChatAppService
    {
        Task<RetornoGenerico> BuscarChat(Guid chatId);
        Task<RetornoGenerico> BuscarTodasMensagensChat(Guid chatId);
        Task<RetornoGenerico> SalvarMensagemChat(CriarMensagemChatDTO mensagemChat);
        Task<RetornoGenerico> CriarChat(CriarChatDTO chat);
    }
}
