namespace Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions
{
    public class HashSenha
    {
        public static string HashSenhaUsuario(string senha)
        {
            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(senha, 13);

            return hash;
        }
    }
}
