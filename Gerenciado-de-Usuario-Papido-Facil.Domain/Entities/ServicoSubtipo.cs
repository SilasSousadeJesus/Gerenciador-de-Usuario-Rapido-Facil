namespace Gerenciado_de_Usuario_Papido_Facil.Domain.Entities
{
    public class ServicoSubtipo
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Guid ServicoId { get; set; }
        //public Servico Servico { get; set; }

        public List<EmpresaPrestadoraServicoSubtipo> EmpresaPrestadoraServicoSubtipos { get; set; } = new List<EmpresaPrestadoraServicoSubtipo>();
    }
}
