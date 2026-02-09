using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel
{
    public class IdoneidadeViewModel
    {
        public Guid Id { get; set; }
        public bool Idoneo { get; set; }
        public EAnaliseIdoneidade EIdoneidade { get; set; }
        public Guid EmpresaPrestadoraId { get; set; }
        public DateTime? DataUltimaAnalise { get; set; }

        public List<CndViewModel> CND { get; set; } = new();
        public List<CndtViewModel> CNDT { get; set; } = new();

        public bool PrecisaEnviarDocumentoCND { get; set; } = true;
        public bool PrecisaEnviarDocumentoCNDT { get; set; } = true;

        public IdoneidadeViewModel() { }
        public IdoneidadeViewModel(Idoneidade entity)
        {
            Id = entity.Id;
            Idoneo = entity.Idoneo;
            EIdoneidade = entity.EIdoneidade;
            EmpresaPrestadoraId = entity.EmpresaPrestadoraId;
            DataUltimaAnalise = entity.DataUltimaAnalise;

            CND = entity.CND?
                .Select(c => new CndViewModel(c))
                .ToList() ?? new();

            CNDT = entity.CNDT?
                .Select(c => new CndtViewModel(c))
                .ToList() ?? new();

            // regras de UI
            PrecisaEnviarDocumentoCND = !CND.Any();
            PrecisaEnviarDocumentoCNDT = !CNDT.Any();
        }

        public void VerificarDocumentos()
        {
            PrecisaEnviarDocumentoCND = PrecisaReenviarDocumento(CND);
            PrecisaEnviarDocumentoCNDT = PrecisaReenviarDocumento(CNDT);
        }

        private bool PrecisaReenviarDocumento<T>(List<T> documentos) where T : DocumentoBaseViewModel
        {
            if (documentos == null || !documentos.Any())
                return true;

            var documentoMaisRecente = documentos
                .OrderByDescending(d => d.DataEnvio)
                .First();

            if (documentoMaisRecente.Tipo != ETipoCertidao.Positiva)
                return true;

            if (!documentoMaisRecente.DataValidade.HasValue)
                return true;

            if (DateTime.Now.Date > documentoMaisRecente.DataValidade.Value.Date)
                return true;

            return false;
        }
 
    }

    public class CndtViewModel : DocumentoBaseViewModel
    {
        public Guid Id { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataRejeicao { get; set; }
        public ETipoCertidao Tipo { get; set; }
        public EEspecieCertidao Especie { get; set; }
        public Guid IdoneidadeId { get; set; }
        public string CnpjCpf { get; set; } = string.Empty;

        public CndtViewModel() { }

        public CndtViewModel(CNDT entity)
        {
            Id = entity.Id;
            DataEmissao = entity.DataExpedicao;
            DataValidade = entity.DataValidade;
            DataEnvio = entity.DataEnvio;
            DataAprovacao = entity.DataAprovacao;
            DataRejeicao = entity.DataRejeicao;
            Tipo = entity.Tipo;
            Especie = entity.Especie;
            IdoneidadeId = entity.IdoneidadeId;
            CnpjCpf = entity.CnpjCpf;
        }
    }
    public class CndViewModel : DocumentoBaseViewModel
    {
        public Guid Id { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataValidade { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime? DataRejeicao { get; set; }
        public ETipoCertidao Tipo { get; set; }
        public EEspecieCertidao Especie { get; set; }
        public Guid IdoneidadeId { get; set; }
        public string CnpjCpf { get; set; } = string.Empty;

        public CndViewModel() { }

        public CndViewModel(CND entity)
        {
            Id = entity.Id;
            DataEmissao = entity.DataEmissao;
            DataValidade = entity.DataValidade;
            DataEnvio = entity.DataEnvio;
            DataAprovacao = entity.DataAprovacao;
            DataRejeicao = entity.DataRejeicao;
            Tipo = entity.Tipo;
            Especie = entity.Especie;
            IdoneidadeId = entity.IdoneidadeId;
            CnpjCpf = entity.CnpjCpf;
        }
    }

    public abstract class DocumentoBaseViewModel
    {
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataValidade { get; set; }
        public ETipoCertidao Tipo { get; set; }
    }
}
