
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces
{
    public interface IEmpresaPrestadoraRepository
    {
        Task<EmpresaPrestadoraViewModel?> BuscarEmpresa(Guid empresaId);
        Task<EmpresaPrestadora?> BuscarEmpresaCompleto(Guid empresaId);
        Task<EmpresaPrestadora?> CadastrarEmpresaAsync(EmpresaPrestadora empresa);
        Task<List<EmpresaPrestadora>> BuscarTodosOsEmpresaAsync();
        Task DeletarEmpresaAsync(EmpresaPrestadora empresa);
        Task EditarEmpresaAsync(EmpresaPrestadora empresaPrestadora);
        Task<PaginacaoDeResultados> BuscarPrestadorPorFiltros(string nome, string cidade, string estado, int pagina, int itensPorPagina, Guid servicoId, List<Guid> servicoSubtipoIds);
        Task<EmpresaPrestadoraComServicoESubServico?> BuscarPrestadorPorEmail(string email);
    }
}
