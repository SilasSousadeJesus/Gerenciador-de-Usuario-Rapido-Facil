namespace Gerenciado_de_Usuario_Papido_Facil.Application.DTOs
{
    public class FinalizarCadastroEmpresaPrestadora
    {
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public bool? Ativo { get; set; } = null;
        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;

        public List<EmpresaPrestadoraServicoSubtipoDTO> Servicos { get; set; } = new List<EmpresaPrestadoraServicoSubtipoDTO>();
    }
}
