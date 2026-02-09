using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Repositories
{
    public class EmpresaPrestadoraRepository : IEmpresaPrestadoraRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;

        public EmpresaPrestadoraRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<EmpresaPrestadoraViewModel?> BuscarEmpresa(Guid empresaId)
        {
            try
            {
                var empresaPrestadora = await _context.Set<EmpresaPrestadora>().Where(x => x.Id == empresaId)
                    .Select(e => new EmpresaPrestadoraViewModel
                    {
                        Id = e.Id,
                        Nome = e.Nome,
                        Senha = string.Empty,
                        CnpjCpf = e.CnpjCpf,
                        Email = e.Email,
                        Logo = e.Logo,
                        Ativo = e.Ativo,
                        Rua = e.Rua,
                        Bairro = e.Bairro,
                        Cidade = e.Cidade,
                        Estado = e.Estado,
                        Cep = e.Cep
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (empresaPrestadora is null) return null;

                var servicosEsubServicos = await _context.EmpresaPrestadoraServicos
                                     .Where(x => x.EmpresaPrestadoraId == empresaId)
                                     .Include(x => x.Servico)
                                     .Include(x => x.ServicoSubtipo)
                                     .GroupBy(x => new { x.ServicoId, x.Servico.Nome })
                                     .Select(g => new viewModel
                                     {
                                         ServicoId = g.Key.ServicoId,
                                         Nome = g.Key.Nome,
                                         EmpresaPrestadoraId = empresaId,
                                         ServicosSubtipos = g.Select(s => new SubTipo
                                         {
                                             ServicoSubtipoId = s.ServicoSubtipoId,
                                             Nome = s.ServicoSubtipo.Nome,
                                             ServicoId = s.ServicoId,
                                             EmpresaPrestadoraId = empresaId
                                         }).ToList()
                                     })
                                     .ToListAsync();


                empresaPrestadora.EmpresaPrestadoraServicoSubtipos = servicosEsubServicos;

                return empresaPrestadora;

            }
            catch (Exception ex)
            {
                // Log da exceção para análise mais detalhada
                Console.WriteLine($"Erro ao buscar empresa: {ex.Message}");
                throw; // Re-throw exception after logging
            }
        }

        public async Task<EmpresaPrestadora?> BuscarEmpresaCompleto(Guid empresaId)
        {
                return await _context.Set<EmpresaPrestadora>().Where(x => x.Id == empresaId)
                    .Select(e => new EmpresaPrestadora
                    {
                        Id = e.Id,
                        Nome = e.Nome,
                        Senha = e.Senha,
                        CnpjCpf = e.CnpjCpf,
                        Email = e.Email,
                        Logo = e.Logo,
                        Ativo = e.Ativo,
                        Rua = e.Rua,
                        Bairro = e.Bairro,
                        Cidade = e.Cidade,
                        Estado = e.Estado,
                        Cep = e.Cep,
                        EmpresaPrestadoraServicoSubtipos = e.EmpresaPrestadoraServicoSubtipos
                    })
                    .FirstOrDefaultAsync();
        }

        public async Task<EmpresaPrestadora?> CadastrarEmpresaAsync(EmpresaPrestadora empresa)
        {
            var entidade = await _context.Set<EmpresaPrestadora>().AddAsync(empresa);
            await _context.SaveChangesAsync();
            var novoEmpresaPrestadora = entidade.Entity;
            return novoEmpresaPrestadora;
        }

        public async Task<List<EmpresaPrestadora>> BuscarTodosOsEmpresaAsync()
        {
            return await _context.Set<EmpresaPrestadora>()
            .Select(e => new EmpresaPrestadora
            {
                Id = e.Id,
                Nome = e.Nome,
                Senha = string.Empty,
                CnpjCpf = e.CnpjCpf,
                Email = e.Email,
                Logo = e.Logo,
                Ativo = e.Ativo,
                Rua = e.Rua,
                Bairro = e.Bairro,
                Cidade = e.Cidade,
                Estado = e.Estado,
                EmpresaPrestadoraServicoSubtipos = e.EmpresaPrestadoraServicoSubtipos
            })
            .ToListAsync();
        }

        public async Task DeletarEmpresaAsync(EmpresaPrestadora empresa)
        {
            _context.Set<EmpresaPrestadora>().Remove(empresa);
            await _context.SaveChangesAsync();
        }

        public async Task EditarEmpresaAsync(EmpresaPrestadora empresaPrestadora)
        {
            var existingEntity = _context.Set<EmpresaPrestadora>().Local.FirstOrDefault(b => b.Id == empresaPrestadora.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Set<EmpresaPrestadora>().Update(empresaPrestadora);
            await _context.SaveChangesAsync();
        }
        public async Task<PaginacaoDeResultados> BuscarPrestadorPorFiltros(string nome, string cidade, string estado, int pagina, int itensPorPagina,  Guid servicoId, List<Guid> servicoSubtipoIds)
        {

            var empresas = await _context.EmpresaPrestadora
            .Where(x =>
                (x.Ativo) &&
                (string.IsNullOrEmpty(nome) || x.Nome.ToLower().Contains(nome.ToLower())) &&
                (string.IsNullOrEmpty(cidade) || x.Cidade == cidade) &&
                (string.IsNullOrEmpty(estado) || x.Estado == estado) &&
                (servicoId == Guid.Empty || x.EmpresaPrestadoraServicoSubtipos.Any(eps => eps.ServicoId == servicoId)) &&
                (!servicoSubtipoIds.Any() ||
                x.EmpresaPrestadoraServicoSubtipos
                    .Where(eps => servicoSubtipoIds.Contains(eps.ServicoSubtipoId))
                    .Count() == servicoSubtipoIds.Count))
            .Include(x => x.EmpresaPrestadoraServicoSubtipos)
            .ToListAsync();

            var listaEmpresaComServicos = new List<EmpresaPrestadoraComServicoESubServico>();

            foreach (var empresa in empresas)
            {
                var empresaCorrente = new EmpresaPrestadoraComServicoESubServico()
                {
                    Ativo = empresa.Ativo,
                    Bairro = empresa.Bairro,
                    Cidade = empresa.Cidade,
                    CnpjCpf = empresa.CnpjCpf,
                    Email = empresa.Email,
                    Estado = empresa.Estado,
                    Id = empresa.Id,
                    Logo = empresa.Logo,
                    Nome = empresa.Nome,
                    Rua = empresa.Rua,
                    Senha = string.Empty,
                    Servicos = new List<Servico>()
                };

                if (empresa.EmpresaPrestadoraServicoSubtipos.Any())
                {
                    foreach (var item in empresa.EmpresaPrestadoraServicoSubtipos)
                    {
                        item.EmpresaPrestadora = null; // Remove a referência circular.
                    }

                    var servicosAgrupadosporId = empresa.EmpresaPrestadoraServicoSubtipos
                                                      .GroupBy(x => x.ServicoId)
                                                      .ToList();

                    foreach (var grupo in servicosAgrupadosporId)
                    {
                        var idservico = Guid.Parse(grupo.Key.ToString());
                        var servico = _context.Servicos.FirstOrDefault(x => x.Id == idservico);

                        if (servico != null)
                        {

                            var servicoSubTiposID = grupo.Select(x => x.ServicoSubtipoId).Distinct().ToList();

                            var servicoSubTipos = _context.ServicoSubtipos
                                                          .Where(s => servicoSubTiposID.Contains(s.Id))
                                                          .ToList();

                            foreach (var sub in servicoSubTipos)
                            {
                                if (!servico.ServicoSubtipos.Any(s => s.Id == sub.Id))
                                {
                                    servico.ServicoSubtipos.Add(sub);
                                }
                            }

                            if (!empresaCorrente.Servicos.Any(s => s.Id == servico.Id))
                            {
                                empresaCorrente.Servicos.Add(servico);
                            }
                        }
                    }
                }

                foreach (var item in empresaCorrente.Servicos)
                {
                    item.EmpresaPrestadoraServicos = null;

                    foreach (var sub in item.ServicoSubtipos)
                    {
                        sub.EmpresaPrestadoraServicoSubtipos = null;
                    }
                }

                listaEmpresaComServicos.Add(empresaCorrente);
            }

            List<dynamic> lista= listaEmpresaComServicos.Cast<dynamic>().ToList();

            PaginacaoDeResultados paginado = PaginacaoDeResultados.PaginacaoHelper.Paginate(lista, pagina, itensPorPagina);

            return paginado;
        }

        public async Task<EmpresaPrestadoraComServicoESubServico?> BuscarPrestadorPorEmail(string email)
        {
            var prestador = await _context.EmpresaPrestadora
             .Where(x => x.Email == email)
            .Select(e => new EmpresaPrestadora
            {
                Id = e.Id,
                Nome = e.Nome,
                Senha = e.Senha,
                CnpjCpf = e.CnpjCpf,
                Email = e.Email,
                Logo = e.Logo,
                Ativo = e.Ativo,
                Rua = e.Rua,
                Bairro = e.Bairro,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Cep = e.Cep,
                EmpresaPrestadoraServicoSubtipos = e.EmpresaPrestadoraServicoSubtipos
            })
            .FirstOrDefaultAsync();

            if (prestador is null) {
                return null;
            }

            var idoneidade = await _context.Idoneidade
                .Where(x => x.EmpresaPrestadoraId == prestador.Id)
                .Select(e => new { e.Idoneo, e.EIdoneidade }) 
                .FirstOrDefaultAsync();

            var empresaCorrente = new EmpresaPrestadoraComServicoESubServico()
            {
                Ativo = prestador.Ativo,
                Bairro = prestador.Bairro,
                Cidade = prestador.Cidade,
                CnpjCpf = prestador.CnpjCpf,
                Email = prestador.Email,
                Estado = prestador.Estado,
                Id = prestador.Id,
                Logo = prestador.Logo,
                Nome = prestador.Nome,
                Rua = prestador.Rua,
                Senha = prestador.Senha,
                Idoneo = idoneidade.Idoneo,
                AnaliseIdoneidade = idoneidade.EIdoneidade,
                Servicos = new List<Servico>()
            };


            if (prestador.EmpresaPrestadoraServicoSubtipos.Any())
            {
                foreach (var item in prestador.EmpresaPrestadoraServicoSubtipos)
                {
                    item.EmpresaPrestadora = null; // Remove a referência circular.
                }


                var servicosAgrupadosporId = prestador.EmpresaPrestadoraServicoSubtipos
                                                  .GroupBy(x => x.ServicoId)
                                                  .ToList();


                foreach (var grupo in servicosAgrupadosporId)
                {
                    var idservico = Guid.Parse(grupo.Key.ToString());
                    var servico = _context.Servicos.FirstOrDefault(x => x.Id == idservico);

                    if (servico != null)
                    {

                        var servicoSubTiposID = grupo.Select(x => x.ServicoSubtipoId).Distinct().ToList();

                        var servicoSubTipos = _context.ServicoSubtipos
                                                      .Where(s => servicoSubTiposID.Contains(s.Id))
                                                      .ToList();

                        foreach (var sub in servicoSubTipos)
                        {
                            if (!servico.ServicoSubtipos.Any(s => s.Id == sub.Id))
                            {
                                servico.ServicoSubtipos.Add(sub);
                            }
                        }

                        if (!empresaCorrente.Servicos.Any(s => s.Id == servico.Id))
                        {
                            empresaCorrente.Servicos.Add(servico);
                        }
                    }
                }
            }

            foreach (var item in empresaCorrente.Servicos)
            {
                item.EmpresaPrestadoraServicos = null;

                foreach (var sub in item.ServicoSubtipos)
                {
                    sub.EmpresaPrestadoraServicoSubtipos = null;
                }
            }

            return empresaCorrente;

        }

    }
}
