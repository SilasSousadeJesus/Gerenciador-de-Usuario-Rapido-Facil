using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.DTOs
{
    public class EmpresaPrestadoraServicoSubtipoDTO
    {
        public Guid? EmpresaPrestadoraId { get; set; } = null;

        public Guid ServicoId { get; set; } = Guid.Empty;

        public Guid ServicoSubtipoId { get; set; } = Guid.Empty;
    }
}
