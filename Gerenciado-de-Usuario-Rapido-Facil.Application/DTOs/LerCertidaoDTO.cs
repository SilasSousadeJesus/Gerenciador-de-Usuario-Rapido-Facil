using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs
{
    public class LerCertidaoDTO
    {
        public string PdfBase64 { get; set; } = string.Empty;
        public EEspecieCertidao EspecieCertidao { get; set; } = EEspecieCertidao.NaoIdentificado;
    }
}
