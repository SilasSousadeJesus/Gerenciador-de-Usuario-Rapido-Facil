namespace Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities
{
    public class Servico
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<ServicoSubtipo> ServicoSubtipos { get; set; } = new List<ServicoSubtipo>();

        public List<EmpresaPrestadoraServicoSubtipo> EmpresaPrestadoraServicos { get; set; } = new List<EmpresaPrestadoraServicoSubtipo>();
    }
}
