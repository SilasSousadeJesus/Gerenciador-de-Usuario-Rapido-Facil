namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface I_IdentificacaoRepository
    {
        Task<string> IdentificacaoUsuario(string email);
    }
}
