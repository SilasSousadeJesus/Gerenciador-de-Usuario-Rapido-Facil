namespace Gerenciado_de_Usuario_Papido_Facil.Application.ViewModel
{
    public class MessageChatViewModel
    {
        public Guid Id { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid ChatId { get; set; }
    }
}
