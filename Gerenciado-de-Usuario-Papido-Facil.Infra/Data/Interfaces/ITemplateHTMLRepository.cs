using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces
{
    public interface ITemplateHTMLRepository
    {
        Task<TemplateHTML?> BuscarTemplateHTMLAsync(Guid id);
        Task<bool> CadastrarTemplateHTMLAsync(TemplateHTML TemplateHTML);
    }
}
