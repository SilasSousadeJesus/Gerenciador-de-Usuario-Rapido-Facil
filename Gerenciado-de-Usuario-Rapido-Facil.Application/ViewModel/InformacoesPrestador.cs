using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel
{
    public class InformacoesPrestador
    {
        public string Nome { get; set; } = string.Empty;
        public int QtServicoPrestados { get; set; } = 0;
        public decimal Avaliacao { get; set; } = 0.0m;
        public DateTime DataCadastro { get; set; }
        public bool Verificada { get; set; }
        public List<viewModel> EmpresaPrestadoraServicoSubtipos { get; set; } = new List<viewModel>();
    }
}
