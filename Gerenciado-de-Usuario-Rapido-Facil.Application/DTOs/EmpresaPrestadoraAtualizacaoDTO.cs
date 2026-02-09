namespace Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs
{
    public class EmpresaPrestadoraAtualizacaoDTO
    {
        public string? Nome { get; set; }
        public string? CnpjCpf { get; set; }
        public string? Email { get; set; }
        public string? Logo { get; set; }
        public bool? Ativo { get; set; }  // Agora é nullable
        public string? Rua { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Cep { get; set; }
    }
}
