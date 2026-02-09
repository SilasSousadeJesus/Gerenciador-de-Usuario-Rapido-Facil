using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Repositories
{
    public class IdentificacaoRepository : I_IdentificacaoRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;

        public IdentificacaoRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<string> IdentificacaoUsuario(string email)
        {
             var identificacao = string.Empty;
            try
            {

                var condominio = await _context.Condominio.Where(x => x.Email == email).FirstOrDefaultAsync();
                var condomino = await _context.Condomino.Where(x => x.Email == email).FirstOrDefaultAsync();
                var prestador = await _context.EmpresaPrestadora.Where(x => x.Email == email).FirstOrDefaultAsync();


                if (condominio != null) identificacao = "Condominio";
                if (condomino != null) identificacao = "Condomino";
                if (prestador != null) identificacao = "Prestador";


                return identificacao;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

    }
}
