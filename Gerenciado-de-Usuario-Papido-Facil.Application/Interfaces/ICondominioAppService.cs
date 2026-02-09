using Gerenciado_de_Usuario_Papido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Papido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces
{
    public interface ICondominioAppService
    {
        Task<RetornoGenerico> CadastrarCondominioAsync(CadastroCondominioDTO condominioDTO);
        Task<RetornoGenerico> BuscarTodosOsCondominioAsync();
        Task<RetornoGenerico> BuscarUmCondominioAsync(Guid condominioId);
        Task<CondominioViewModel?> BuscarUmCondominioPorEmailAsync(string email);
        Task<RetornoGenerico> DeletarCondominioAsync(Guid condominioId);
        Task<RetornoGenerico> EditarCondominioAsync(Guid condominioId, EdicaoCondominioDTO edicaoCondominioDTO);
        Task<RetornoGenerico> BuscarUmCondominioPorCodigoVinculacaoAsync(string codigovalidacao);
        Task<RetornoGenerico> FinalizarPeriodoTeste(Guid condominioId);
        Task<RetornoGenerico> EnviarEmailVincularCondomino(string emailAVincular, Guid usuarioIdRemetente);
        Task<RetornoGenerico> TrocarSenha(Guid condominioId, TrocaSenhaDTO trocaSenhaDTO);
    }
}
