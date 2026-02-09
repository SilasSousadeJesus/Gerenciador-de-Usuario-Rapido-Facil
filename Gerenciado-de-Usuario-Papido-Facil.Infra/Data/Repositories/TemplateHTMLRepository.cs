using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Repositories
{
    public class TemplateHTMLRepository : ITemplateHTMLRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;

        public TemplateHTMLRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<TemplateHTML?> BuscarTemplateHTMLAsync(Guid id)
        {
            return await _context.Set<TemplateHTML>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CadastrarTemplateHTMLAsync(TemplateHTML TemplateHTML)
        {
            var entidade = await _context.Set<TemplateHTML>().AddAsync(TemplateHTML);
            var resultado = await _context.SaveChangesAsync();
            return resultado > 0;
        }
    }
}
