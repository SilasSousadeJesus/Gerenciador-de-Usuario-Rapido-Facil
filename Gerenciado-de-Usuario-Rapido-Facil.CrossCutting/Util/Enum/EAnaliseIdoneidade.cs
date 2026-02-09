using System.ComponentModel;

namespace Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum
{
    public enum EAnaliseIdoneidade
    {
        [Description("Quando a Analise ainda não foi iniciada")]
        NaoAnalisada,
        [Description("Analise em Andamento")]
        EmAnalise,               
        [Description("Quando as informações para analise nao foram fornecidas")]
        Pendente,       
        [Description("Analise completada: identifica o prestador como idoneo ou nao")]
        Analisada,
    }
}
