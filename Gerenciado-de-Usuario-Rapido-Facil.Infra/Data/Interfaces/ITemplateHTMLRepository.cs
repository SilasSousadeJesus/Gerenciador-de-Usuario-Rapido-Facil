using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces
{
    public interface ITemplateHTMLRepository
    {
        Task<TemplateHTML?> BuscarTemplateHTMLAsync(Guid id);
        Task<bool> CadastrarTemplateHTMLAsync(TemplateHTML TemplateHTML);
    }
}
