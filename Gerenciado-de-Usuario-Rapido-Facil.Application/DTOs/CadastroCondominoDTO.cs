namespace Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs
{
    public class CadastroCondominoDTO
    { 
        public string Nome { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CodigoVinculo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public bool AceitouTermosDeUso { get; set; } = false;
    }
}
