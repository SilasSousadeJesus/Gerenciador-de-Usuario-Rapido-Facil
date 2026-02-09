namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces
{
    public interface IValidacaoParaCadastroRepository
    {
        Task<bool> VerificarEmailExiste(string email);
    }
}
