
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class Chat
    {
        public Chat(Guid empresaPrestadoraId, Guid? condominioId = null, Guid? condominoId = null)
        {
            CondominioId = condominioId;
            CondominoId = condominoId;
            EmpresaPrestadoraId  = empresaPrestadoraId;
        }

        public Chat()
        {
        }

        public Guid Id { get; set; }
        public Guid? CondominioId{ get; set; }
        public Guid? CondominoId{ get; set; }
        public Guid EmpresaPrestadoraId { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public virtual List<MensagemChat> mensagens { get; set; }

        public bool ValidandoSolicitante() {      
            if(CondominioId is null && CondominoId is null) return false;         
            if(CondominioId != null && CondominoId != null) return false;         
            return true;   
        }


        public ETipoUsuario IdentificandoSolicitante()
        {
            var validando =  ValidandoSolicitante();

            if(!validando) return ETipoUsuario.NaoIdentificado;

            if (CondominioId != null) return ETipoUsuario.Condominio;
            if (CondominoId != null) return ETipoUsuario.Condomino;

            return ETipoUsuario.NaoIdentificado;
        }
    }
}
