
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface IAnaliseIdoneidadeAppService
    {
        Task<RetornoGenerico> CriarIdoneidade(Guid empresaId);
        Task<RetornoGenerico> LerCertidoes(Guid empresaId, string pdfBase64, EEspecieCertidao eEspecieCertidao);
        Task<RetornoGenerico> BuscarIdoneidade(Guid empresaId);
    }
}
