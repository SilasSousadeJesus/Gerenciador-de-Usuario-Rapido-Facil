namespace Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces
{
    public interface IEmailAppService
    {
        Task EnviarEmailVinculo(string destinatario, string msg, string testbody);
    }
}
