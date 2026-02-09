using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Repositories
{
    public class CondominoRepository : ICondominoRepository
    {

        private readonly GerenciadorUsuarioDbContext _context;

        public CondominoRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }
        public async Task<PaginacaoDeResultados> BuscarTodosOsCondominosAsync(Guid condominioId, string nome, string email, bool? ativo, DateTime dataCadastro, int pagina, int itensPorPagina)
        {
            var query = _context.Set<Condomino>()
                .AsNoTracking()
                .Where(c => c.CondominioId == condominioId &&
                            (string.IsNullOrEmpty(nome) || c.Nome.Contains(nome)) &&
                            (string.IsNullOrEmpty(email) || c.Email.Contains(email)) &&
                            (ativo == null || c.Ativo == ativo.Value) &&
                            (dataCadastro == default(DateTime) || c.DataCadastro.Date == dataCadastro.Date));

            var condominos = await query
                .Select(c => new Condomino
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    CnpjCpf = c.CnpjCpf,
                    Email = c.Email,
                    Senha = string.Empty,
                    Ativo = c.Ativo,
                    Rua = c.Rua,
                    Bairro = c.Bairro,
                    Cidade = c.Cidade,
                    Estado = c.Estado,
                    Vinculado = c.Vinculado,
                    CodigoVinculacao = c.CodigoVinculacao,
                    CondominioId = c.CondominioId
                }).ToListAsync();

            var dynamicList = condominos.Cast<dynamic>().ToList();

            return PaginacaoDeResultados.PaginacaoHelper.Paginate(
                dynamicList,
                pagina,
                itensPorPagina);
        }

        public async Task<Condomino?> BuscarUmCondominoAsync(Guid condominoId)
        {
            return await _context.Set<Condomino>()
                .Where(x => x.Id == condominoId)
                .Select(c => new Condomino
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    CnpjCpf = c.CnpjCpf,
                    Email = c.Email,
                    Senha = string.Empty,
                    Ativo = c.Ativo,
                    Rua = c.Rua,
                    Bairro = c.Bairro,
                    Cidade = c.Cidade,
                    Estado = c.Estado,
                    Vinculado = c.Vinculado,
                    CodigoVinculacao = c.CodigoVinculacao,
                    Cep = c.Cep,
                    CondominioId = c.CondominioId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Condomino?> BuscarUmCondominoCompletoAsync(Guid condominoId)
        {
            return await _context.Set<Condomino>()
                .Where(x => x.Id == condominoId)
                .Select(c => new Condomino
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    CnpjCpf = c.CnpjCpf,
                    Email = c.Email,
                    Senha = c.Senha,
                    Ativo = c.Ativo,
                    Rua = c.Rua,
                    Bairro = c.Bairro,
                    Cidade = c.Cidade,
                    Estado = c.Estado,
                    Vinculado = c.Vinculado,
                    Cep = c.Cep,
                    CodigoVinculacao = c.CodigoVinculacao,
                    CondominioId = c.CondominioId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Condomino?> CadastrarCondominoAsync(Condomino condomino)
        {
            var entidade = await _context.Set<Condomino>().AddAsync(condomino);
            await _context.SaveChangesAsync();
            var novoCondomino = entidade.Entity;
            return novoCondomino;
        }

        public async Task DeletarCondominoAsync(Condomino condomino)
        {
            _context.Set<Condomino>().Remove(condomino);

            await _context.SaveChangesAsync();
        }

        public async Task EditarCondominoAsync(Condomino condomino)
        {
            var existingEntity = _context.Set<Condomino>().Local.FirstOrDefault(b => b.Id == condomino.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Set<Condomino>().Update(condomino);
            await _context.SaveChangesAsync();
        }

        public async Task<Condomino?> BuscarCondominoPorEmail(string email)
        {
            return await _context.Condomino.Where(x => x.Email == email).FirstOrDefaultAsync();
        }
    }
}
