using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface IBuscaInformacoesExternas
    {
        Task<RetornoGenerico> ObterEstadosAsync();

        Task<RetornoGenerico> ObterMunicipiosAsync(string estadoSigla);
    }
}
