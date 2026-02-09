namespace Gerenciado_de_Usuario_Papido_Facil.Domain.Entities
{
    public class EmpresaPrestadoraServicoSubtipo
    {
        public Guid EmpresaPrestadoraId { get; set; }
        public EmpresaPrestadora EmpresaPrestadora { get; set; }

        public Guid ServicoId { get; set; }
        public Servico Servico { get; set; }

        public Guid ServicoSubtipoId { get; set; }
        public ServicoSubtipo ServicoSubtipo { get; set; }
    }
}
