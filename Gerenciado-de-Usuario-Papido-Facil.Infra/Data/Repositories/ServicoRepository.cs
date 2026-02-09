using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Repositories
{
    public class ServicoRepository : IServicoRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;
 
        public ServicoRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }
        public async Task<bool> GeracaoDeServicoServicoSubTipoAsync(List<Servico> listaServico)
        {
           var existeServicos =  await BuscarTodosOsServicos();

           if (existeServicos.Any()) return false;

           await _context.Servicos.AddRangeAsync(listaServico);
           var resultado = await _context.SaveChangesAsync();

            _context.ProcedureCopiarServicoSubtipo();

           return resultado > 0;
        }
        public async Task<List<Servico>> BuscarTodosOsServicos()
        {
            return await _context.Set<Servico>().Include(x => x.ServicoSubtipos).ToListAsync();
        }
        public async Task<Servico?> BuscarServico(Guid servicoId)
        {
            return await _context.Set<Servico>().Where(x => x.Id == servicoId)
                                                                .Include(x => x.ServicoSubtipos)
                                                                                                .FirstOrDefaultAsync();
        }
        public async Task<bool> AdicionarServicoAEmpresa(Guid empresaId, List<EmpresaPrestadoraServicoSubtipo> empresaPrestadoraServicoSubtipos)
        {
            try
            {
                // Busca os registros existentes para a empresa
                var registrosExistentes = await _context.Set<EmpresaPrestadoraServicoSubtipo>()
                                                         .Where(x => x.EmpresaPrestadoraId == empresaId)
                                                         .ToListAsync();

                // Verifica quais itens são novos (não existem no banco)
                var itensNovos = empresaPrestadoraServicoSubtipos
                    .Where(dto => !registrosExistentes.Any(x => x.EmpresaPrestadoraId == dto.EmpresaPrestadoraId
                                                             && x.ServicoSubtipoId == dto.ServicoSubtipoId
                                                             && x.ServicoId == dto.ServicoId))
                    .ToList();

                // Se todos os itens forem novos, deleta os registros existentes e salva os novos
                if (itensNovos.Count == empresaPrestadoraServicoSubtipos.Count)
                {
                    _context.Set<EmpresaPrestadoraServicoSubtipo>().RemoveRange(registrosExistentes);
                    await _context.AddRangeAsync(itensNovos);
                }
                // Se existirem itens novos, mas também itens já existentes, apenas adiciona os novos
                else if (itensNovos.Any())
                {
                    await _context.AddRangeAsync(itensNovos);
                }

                // se houver mais registros no banco do que enviado, se entende que é para algum ser removido
                else if (registrosExistentes.Count > empresaPrestadoraServicoSubtipos.Count) {
                    foreach (var item in registrosExistentes)
                    {
                        foreach (var dto in empresaPrestadoraServicoSubtipos)
                        {
                            if (item.EmpresaPrestadoraId == dto.EmpresaPrestadoraId && item.ServicoSubtipoId == dto.ServicoSubtipoId && item.ServicoId == dto.ServicoId)
                            {
                                continue;
                            }

                            _context.Set<EmpresaPrestadoraServicoSubtipo>().RemoveRange(item);
                        }
                    }
                }
                // Se todos os itens já existirem, não faz nada
                else
                {
                    return true;  // Todos os itens já existem, então não faz alterações
                }

                // Salva as alterações no banco de dados
                var resultado = await _context.SaveChangesAsync();
                return resultado > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                throw;
            }
        }


        //public async Task<List<Servico>> BuscarTodosOsServicosDeEmpresa(Guid empresaId)
        //{
        //    return await _context.Set<Servico>().Where(x => x.empr).Include(x => x.ServicoSubtipos).ToListAsync();
        //}
    }
}
