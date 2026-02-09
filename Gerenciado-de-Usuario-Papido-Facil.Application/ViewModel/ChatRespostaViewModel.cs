namespace Gerenciado_de_Usuario_Papido_Facil.Application.ViewModel
{
    public class ChatRespostaViewModel
    {
        public Guid Id { get; set; }
        public Guid? CondominioId { get; set; }
        public Guid? CondominoId { get; set; }
        public Guid EmpresaPrestadoraId { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<MessageChatViewModel> Mensagens { get; set; } = new();
    }
}
