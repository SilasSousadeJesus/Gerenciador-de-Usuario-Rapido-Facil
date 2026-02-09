using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.DTOs
{
    public class NotificarUmUsuarioDTO
    {
        public ENotificacao tipoNotificacao { get; set; }  = ENotificacao.cotacao;
        public Guid idRelacionado { get; set; }
    }
}
