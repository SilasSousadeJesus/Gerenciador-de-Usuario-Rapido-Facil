using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface ICondominoAppService
    {
        Task<RetornoGenerico> CadastrarCondominoAsync(CadastroCondominoDTO condominoDTO);
        Task<RetornoGenerico> VincularCondominoACondominioAsync(string codigo, Guid condominoId);
        Task<RetornoGenerico> BuscarUmCondominoAsync(Guid condominoId);
        Task<CondominoViewModel> BuscarUmCondominoPorEmailAsync(string email);
        Task<RetornoGenerico> BuscarTodosOsCondominosAsync(Guid condominioId, BuscarCondominoPorFiltrosDTO filtros);
        Task<RetornoGenerico> DeletarCondominoAsync(Guid condominoId);
        Task<RetornoGenerico> EditarCondominoAsync(Guid condominoId, EdicaoCondominoDTO edicaoCondominoDTO);
        Task<RetornoGenerico> TrocarSenha(Guid condominoId, TrocaSenhaDTO trocaSenhaDTO);
        Task<RetornoGenerico> CadastrarCondominoParaTesteAsync(Guid condominioId, int numeroDeCondomino);
    }
}
