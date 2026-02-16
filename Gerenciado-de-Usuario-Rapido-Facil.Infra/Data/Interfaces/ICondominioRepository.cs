using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface ICondominioRepository
    {
        Task DeletarCondominioAsync(Condominio condominio);

        Task<Condominio?> BuscarPorCodigoVinculacaoAsync(string codigo);

        Task<PaginacaoDeResultados> BuscarTodosOsCondominiosAsync(string nome, string email, string cnpj, string codigoVinculacao, string cidade, string estado, bool? ativo, bool? periodoTeste, DateTime? dataCadastro, int pagina, int itensPorPagina);

        Task<Condominio?> BuscarUmCondominioAsync(Guid condominioId);

        Task<Condominio?> BuscarUmCondominioCompletoAsync(Guid condominioId);

        Task EditarCondominioAsync(Condominio condominio);

        Task<Condominio?> BuscarCondomonioPorEmail(string email);
        Task<Condominio?> CadastrarCondominioAsync(Condominio condominio);
        Task<List<Condomino>> BuscarCondominosDeCondominioAsync(Guid condominioId);
    }
}
