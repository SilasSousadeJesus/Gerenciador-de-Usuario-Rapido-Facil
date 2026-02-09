using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces
{
    public interface ICondominioRepository
    {
        Task DeletarCondominioAsync(Condominio condominio);

        Task<Condominio?> BuscarPorCodigoVinculacaoAsync(string codigo);

        Task<List<Condominio>> BuscarTodosOsCondominiosAsync();

        Task<Condominio?> BuscarUmCondominioAsync(Guid condominioId);

        Task<Condominio?> BuscarUmCondominioCompletoAsync(Guid condominioId);

        Task EditarCondominioAsync(Condominio condominio);

        Task<Condominio?> BuscarCondomonioPorEmail(string email);
        Task<Condominio?> CadastrarCondominioAsync(Condominio condominio);
        Task<List<Condomino>> BuscarCondominosDeCondominioAsync(Guid condominioId);
    }
}
