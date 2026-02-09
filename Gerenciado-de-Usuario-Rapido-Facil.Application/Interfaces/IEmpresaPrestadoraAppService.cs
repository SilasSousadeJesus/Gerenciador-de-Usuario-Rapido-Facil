using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces
{
    public interface IEmpresaPrestadoraAppService
    {
        Task<RetornoGenerico> CadastrarEmpresaAsync(EmpresaPrestadoraDTO empresaDTO);
        Task<RetornoGenerico> BuscarEmpresaAsync(Guid empresaId);
        Task<RetornoGenerico> BuscarEmpresaCompletoAsync(Guid empresaId);
        Task<RetornoGenerico> BuscarTodosAsEmpresasAsync();
        Task<RetornoGenerico> DeletarEmpresaAsync(Guid empresaId);
        Task<RetornoGenerico> EditarEmpresaAsync(Guid empresaId, EmpresaPrestadoraAtualizacaoDTO edicaoEmpresaDTO);
        Task<RetornoGenerico> BuscarPrestadorPorFiltros(BuscarPrestadorPorFiltrosDTO filtros);
        Task<RetornoGenerico> FinalizarCadastroEmpresaPrestadora(Guid empresaId, FinalizarCadastroEmpresaPrestadora finalizarCadastro);
        Task<dynamic> BuscarEmpresaPorEmailAsync(string email);
        Task<RetornoGenerico> CadastrarEmpresasParaTesteAsync(int numeroDePrestadores);
        Task<RetornoGenerico> VincularEmpresaAServicoSubServico(Guid empresaId, List<EmpresaPrestadoraServicoSubtipoDTO> listaServicosSubServicos);
        Task<RetornoGenerico> AgruparInformacoesPrestador(Guid empresaId);
        Task<RetornoGenerico> TrocarSenha(Guid empresaId, TrocaSenhaDTO trocaSenhaDTO);
    }
}
