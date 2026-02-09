
using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces
{
    public interface IServicoAppService
    {
        Task<RetornoGenerico> GeracaoDeServicoServicoSubTipoAsync();
        Task<RetornoGenerico> BuscarTodosOsServicosAsync();
        Task<RetornoGenerico> VincularServicoAEmpresa(Guid empresaId, List<EmpresaPrestadoraServicoSubtipoDTO> listaEmpresaPrestadoraServicoSubtipoDTO);
    }
}
