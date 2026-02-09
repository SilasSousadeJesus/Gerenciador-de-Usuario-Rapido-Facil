namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class EmpresaPrestadoraViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CnpjCpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public string Rua { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public List<viewModel> EmpresaPrestadoraServicoSubtipos { get; set; } = new List<viewModel>();
    }

    public class viewModel
    {

        public Guid ServicoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<SubTipo> ServicosSubtipos { get; set; }
        public Guid EmpresaPrestadoraId { get; set; }
    }

    public class SubTipo
    {
        public Guid ServicoSubtipoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Guid ServicoId { get; set; }
        public Guid EmpresaPrestadoraId { get; set; }
    }
}
