using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Repositories
{
    public class CondominioRepository : ICondominioRepository
    {

        private readonly GerenciadorUsuarioDbContext _context;

        public CondominioRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Condominio>> BuscarTodosOsCondominiosAsync()
        {
            return await _context.Set<Condominio>()
                                          .Select(c => new Condominio
                                          {
                                              Id = c.Id,
                                              Nome = c.Nome,
                                              CnpjCpf = c.CnpjCpf,
                                              Email = c.Email,
                                              Senha = string.Empty, 
                                              Ativo = c.Ativo,
                                              CodigoVinculacao = c.CodigoVinculacao,
                                              Rua = c.Rua,
                                              Bairro = c.Bairro,
                                              Cidade = c.Cidade,
                                              Estado = c.Estado
                                          })
                                          .ToListAsync();
        }
        public async Task<Condominio?> BuscarUmCondominioAsync(Guid condominioId)
        {
            return await _context.Set<Condominio>().Where(x => x.Id == condominioId)
          .Select(c => new Condominio
            {
                Id = c.Id,
                Nome = c.Nome,
                CnpjCpf = c.CnpjCpf,
                Email = c.Email,
                Senha = string.Empty,
                Ativo = c.Ativo,
                CodigoVinculacao = c.CodigoVinculacao,
                Rua = c.Rua,
                Bairro = c.Bairro,
                Cidade = c.Cidade,
                Estado = c.Estado,
                 Cep = c.Cep,
          }).FirstOrDefaultAsync();
        }

        public async Task<Condominio?> BuscarUmCondominioCompletoAsync(Guid condominioId)
        {
            return await _context.Set<Condominio>().Where(x => x.Id == condominioId)
          .Select(c => new Condominio
          {
              Id = c.Id,
              Nome = c.Nome,
              CnpjCpf = c.CnpjCpf,
              Email = c.Email,
              Senha = c.Senha,
              Ativo = c.Ativo,
              CodigoVinculacao = c.CodigoVinculacao,
              Rua = c.Rua,
              Bairro = c.Bairro,
              Cidade = c.Cidade,
              Estado = c.Estado,
              Cep = c.Cep,
          }).FirstOrDefaultAsync();
        }
        public async Task<Condominio?> CadastrarCondominioAsync(Condominio condominio)
        {
            var entidade = await _context.Set<Condominio>().AddAsync(condominio);
            await _context.SaveChangesAsync();
            var novoCondominio = entidade.Entity;
            return novoCondominio;
        }
        public async Task EditarCondominioAsync(Condominio condominio)
        {
            var existingEntity = _context.Set<Condominio>().Local.FirstOrDefault(b => b.Id == condominio.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Set<Condominio>().Update(condominio);
            await _context.SaveChangesAsync();
        }
        public async Task<Condominio?> BuscarPorCodigoVinculacaoAsync(string codigo)
        {
           return await _context.Condominio.Where(x => x.CodigoVinculacao == codigo.ToUpper()).FirstOrDefaultAsync();
        }
        public async Task DeletarCondominioAsync(Condominio condominio)
        {
            var condominos = await _context.Condomino.Where(x => x.Id == condominio.Id).ToListAsync();

            _context.Condomino.RemoveRange(condominos);

            _context.Set<Condominio>().Remove(condominio);

            await _context.SaveChangesAsync();
        }
        public async Task<Condominio?> BuscarCondomonioPorEmail(string email)
        {
            return await _context.Condominio.Where(x => x.Email == email).FirstOrDefaultAsync();
        }
        public async Task<List<Condomino>> BuscarCondominosDeCondominioAsync(Guid condominioId)
        {
            var condominio = await _context.Set<Condominio>()
                .Where(x => x.Id == condominioId)
                .Include(x => x.Condominos)
                .Select(c => new Condominio
                {
                    Condominos = c.Condominos
                })
                .FirstOrDefaultAsync();

            if (condominio == null || condominio.Condominos == null || !condominio.Condominos.Any())
                return new List<Condomino>();

            var condominos = condominio.Condominos.Select(c => new Condomino
            {
                Id = c.Id,
                Nome = c.Nome,
                CnpjCpf = c.CnpjCpf,
                Email = c.Email,
                Senha = string.Empty,
                Ativo = c.Ativo,
                CodigoVinculacao = c.CodigoVinculacao,
                Rua = c.Rua,
                Bairro = c.Bairro,
                Cidade = c.Cidade,
                Estado = c.Estado,
                CondominioId = condominioId
            }).ToList();

            return condominos;
        }

    }
}
