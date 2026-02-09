using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class Notificacao
    {
        public Notificacao(string mensagem, Guid usuarioId, ENotificacao eNotificacao, Guid idRelacionado)
        {
            Mensagem = mensagem;
            UsuarioId = usuarioId;
            Tipo = eNotificacao;
            IdRelacionado = idRelacionado;
        }

        public Notificacao(string mensagem, Guid usuarioId)
        {
            Mensagem = mensagem;
            UsuarioId = usuarioId;
        }
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        public string Mensagem { get; set; }
        public bool FoiLido { get; set; } = false;
        public ENotificacao Tipo { get; set; }
        public Guid? IdRelacionado { get; set; }
        public Guid UsuarioId { get; set; }


        public void MarcarComoLido()
        {
            FoiLido = true;
            DataAtualizacao = DateTime.Now;
        }
    }
}
