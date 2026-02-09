namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface IValidacaoParaCadastroRepository
    {
        Task<bool> VerificarEmailExiste(string email);
    }
}
