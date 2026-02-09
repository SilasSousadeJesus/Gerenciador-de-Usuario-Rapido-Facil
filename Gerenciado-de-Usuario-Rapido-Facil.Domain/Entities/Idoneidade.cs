using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class Idoneidade
    {
        public Idoneidade()
        {
        }

        public Idoneidade(Guid empresaPrestadoraId)
        {
            EmpresaPrestadoraId = empresaPrestadoraId;
        }

        public Guid Id { get; set; }
        public bool Idoneo { get; set; } = false;
        public EAnaliseIdoneidade EIdoneidade { get; set; } = EAnaliseIdoneidade.NaoAnalisada;
        public Guid EmpresaPrestadoraId { get; set; }




        public void VerificarIdoneidade()
        {
            DataUltimaAnalise = DateTime.Now;

            if (!CND.Any() || !CNDT.Any())
            {
                Idoneo = false;
                return;
            }

            var cndMaisRecente = CND.MaxBy(x => x.DataValidade);
            var cnd = cndMaisRecente.Tipo != ETipoCertidao.Positiva;

            var cndTMaisRecente = CNDT.MaxBy(x => x.DataValidade);
            var cndt = cndTMaisRecente.Tipo != ETipoCertidao.Positiva;

            if (!cnd || !cndt) {
                Idoneo = false;
                return;
            }

            Idoneo = true;
            return;
        }
        public bool VerificarValidadeCertidoes()
        {
            DataUltimaAnalise = DateTime.Now;

            if (!CND.Any()) return true;

            var certidaoMaisRecente = CND.MaxBy(x => x.DataValidade);

            if (DateTime.Now > certidaoMaisRecente.DataValidade)
            {
                return false;
            }

            return true;
        }
        public virtual EmpresaPrestadora EmpresaPrestadora { get; set; }
        public virtual List<CND> CND { get; set; }
        public virtual List<CNDT> CNDT { get; set; }
        public virtual DateTime? DataUltimaAnalise { get; set; }

    }
}
