using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface IServicoRepository
    {
        Task<bool> GeracaoDeServicoServicoSubTipoAsync(List<Servico> listaServico);
        Task<List<Servico>> BuscarTodosOsServicos();
        Task<Servico?> BuscarServico(Guid servicoId);
        Task<bool> AdicionarServicoAEmpresa(Guid empresaId, List<EmpresaPrestadoraServicoSubtipo> empresaPrestadoraServicoSubtipos);
    }
}
