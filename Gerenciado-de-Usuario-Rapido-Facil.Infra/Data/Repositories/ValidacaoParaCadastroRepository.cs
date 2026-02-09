using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Repositories
{
    public class ValidacaoParaCadastroRepository : IValidacaoParaCadastroRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;

        public ValidacaoParaCadastroRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<bool> VerificarEmailExiste(string email)
        {

            var condominio = await _context.Condominio.Where(x => x.Email == email).FirstOrDefaultAsync();
            var condomino = await _context.Condomino.Where(x => x.Email == email).FirstOrDefaultAsync();
            var prestador = await _context.EmpresaPrestadora.Where(x => x.Email == email).FirstOrDefaultAsync();


            if (condominio != null) return true;
            if (condomino != null) return true;
            if (prestador != null) return true;


            return false;

        }
    }
}
