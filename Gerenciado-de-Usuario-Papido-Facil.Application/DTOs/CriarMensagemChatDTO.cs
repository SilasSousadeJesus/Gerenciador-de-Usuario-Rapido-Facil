using System.ComponentModel.DataAnnotations.Schema;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.DTOs
{
    public class CriarMensagemChatDTO
    {
        public string Mensagem { get; set; } = string.Empty;
        public Guid UsuarioId { get; set; }
        public Guid ChatId { get; set; }
    }
}
