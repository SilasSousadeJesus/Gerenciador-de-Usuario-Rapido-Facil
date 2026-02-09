using System.ComponentModel;

namespace Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum
{
    public enum EEspecieCertidao
    {
        [Description("Documento Não Identificado")]
        NaoIdentificado,
        [Description(" CERTIDÃO NEGATIVA DE DÉBITOS")]
        CND,
        [Description(" CERTIDÃO NEGATIVA DE DÉBITOS TRABALHISTAS")]
        CNDT
    }
}
