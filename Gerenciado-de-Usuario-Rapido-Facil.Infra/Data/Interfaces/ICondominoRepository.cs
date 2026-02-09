using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface ICondominoRepository
    {
        Task DeletarCondominoAsync(Condomino condominio);

        Task<PaginacaoDeResultados> BuscarTodosOsCondominosAsync(Guid condominioId, string nome, string email, bool? ativo, DateTime dataCadastro, int pagina, int itensPorPagina);

        Task<Condomino?> BuscarUmCondominoAsync(Guid condominioId);

        Task<Condomino?> BuscarUmCondominoCompletoAsync(Guid condominoId);

        Task<Condomino?> CadastrarCondominoAsync(Condomino condominio);

        Task EditarCondominoAsync(Condomino condominio);

        Task<Condomino?> BuscarCondominoPorEmail(string email);
    }
}
