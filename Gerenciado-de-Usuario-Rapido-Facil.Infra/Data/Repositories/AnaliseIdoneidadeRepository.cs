using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Repositories
{
    public class AnaliseIdoneidadeRepository : IAnaliseIdoneidadeRepository
    {

        private readonly GerenciadorUsuarioDbContext _context;

        public AnaliseIdoneidadeRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }
   
        public async Task<Idoneidade?> BuscarIdoneidadeAsync(Guid empresaId) { 
                return await _context.Set<Idoneidade>().Where(x => x.EmpresaPrestadoraId == empresaId)
                                                            .Include(x => x.CND)
                                                            .Include(x => x.CNDT).FirstOrDefaultAsync();
        }

        public async Task<Idoneidade> CadastrarIdoneidadeAsync(Idoneidade idoneidade)
        {
            var entidade = await _context.Set<Idoneidade>().AddAsync(idoneidade);
            await _context.SaveChangesAsync();
            var novoIdoneidade = entidade.Entity;
            return novoIdoneidade;
        }
  
        public async Task EditarIdoneidadeAsync(Idoneidade idoneidade)
        {
            var existingEntity = _context.Set<Idoneidade>().Local.FirstOrDefault(b => b.Id == idoneidade.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Set<Idoneidade>().Update(idoneidade);
            await _context.SaveChangesAsync();
        }

        public async Task<CND> CadastrarCND(CND certidao)
        {
            var entidade = await _context.Set<CND>().AddAsync(certidao);
            await _context.SaveChangesAsync();
            var novoCertidao = entidade.Entity;
            return novoCertidao;
        }

        public async Task<CNDT> CadastrarCNDT(CNDT certidao)
        {
            var entidade = await _context.Set<CNDT>().AddAsync(certidao);
            await _context.SaveChangesAsync();
            var novoCertidao = entidade.Entity;
            return novoCertidao;
        }
    }
}
