using System.ComponentModel;

namespace Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum
{
    public enum ETipoCertidao
    {
        [Description("")]
        NaoIdentificado,
        [Description("É certificado que não constam pendências em seu nome, relativas a créditos tributários administrados pela Secretaria da Receita Federal do Brasil (RFB) e a inscrições em Dívida Ativa da União (DAU) junto à Procuradoria-Geral da Fazenda Nacional (PGFN)")]
        Negativa,
        [Description("constam débitos administrados com exigibilidade suspensa ou e objeto  de  decisão  judicial  que  determina  sua\r\ndesconsideração para fins de certificação da regularidade fiscal, ou ainda não vencidos")]
        PositivaComfeitoNegativa,
        [Description("")]
        Positiva
    }
}
