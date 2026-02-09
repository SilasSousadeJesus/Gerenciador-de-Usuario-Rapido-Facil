using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Repositories
{
    public class NotificacaoRepository : INotificacaoRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;

        public NotificacaoRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notificacao>> BuscarTodasNotificacoesAsync(Guid usuarioId)
        {
            return await _context.Set<Notificacao>().Where(x => x.UsuarioId == usuarioId).OrderByDescending(x => x.DataCriacao).ToListAsync() ?? [];
        }

        public async Task<Notificacao?> BuscarNotificacaoAsync(Guid id)
        {
            return await _context.Set<Notificacao>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task EditarNotificacaoAsync(Notificacao notificacao)
        {
            var existingEntity = _context.Set<Notificacao>().Local.FirstOrDefault(b => b.Id == notificacao.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Set<Notificacao>().Update(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task EditarTodasNotificacaoAsync(Guid usuarioId)
        {
            var lista = await _context.Set<Notificacao>().Where(x => x.UsuarioId == usuarioId && !x.FoiLido).ToListAsync() ?? [];

            foreach (var notificacao in lista)
            {
                var existingEntity = _context.Set<Notificacao>().Local.FirstOrDefault(b => b.Id == notificacao.Id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                }

                notificacao.MarcarComoLido();
            }

            _context.Set<Notificacao>().UpdateRange(lista);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> CriarNotificacaoAsync(Notificacao notificacao)
        {

            var entidade = await _context.Set<Notificacao>().AddAsync(notificacao);
            var resultado = await _context.SaveChangesAsync();
            var novaNotificao = entidade.Entity;

            if (resultado < 1) return false;

            return true;

        }

        public async Task<bool> CriarNotificacaoAsync(List<Notificacao> lista)
        {
            await _context.Set<Notificacao>().AddRangeAsync(lista);

            var resultado = await _context.SaveChangesAsync();

            if (resultado < 1) return false;

            return true;
        }

        public async Task<bool> NotificarBaseUsuarioAsync(string mensagem, ETipoUsuario eTipoUsuario)
        {
            var idsDaBaseUsuario = new List<Guid>();

            if (eTipoUsuario == ETipoUsuario.Condominio || eTipoUsuario == ETipoUsuario.NaoIdentificado)
            {
                var condominioLista = await _context.Condominio.ToListAsync();
                idsDaBaseUsuario.AddRange(condominioLista.Select(c => c.Id));
            }

            if (eTipoUsuario == ETipoUsuario.Condomino || eTipoUsuario == ETipoUsuario.NaoIdentificado)
            {
                var condominoLista = await _context.Condomino.ToListAsync();
                idsDaBaseUsuario.AddRange(condominoLista.Select(c => c.Id));
            }

            if (eTipoUsuario == ETipoUsuario.EmpresaPrestadora || eTipoUsuario == ETipoUsuario.NaoIdentificado)
            {
                var empresaLista = await _context.EmpresaPrestadora.ToListAsync();
                idsDaBaseUsuario.AddRange(empresaLista.Select(e => e.Id));
            }

            var listaNotificacao = idsDaBaseUsuario.Select(x => new Notificacao(mensagem, x)).ToList();

            if (!listaNotificacao.Any()) return true;

            await _context.Set<Notificacao>().AddRangeAsync(listaNotificacao);

            var resultado = await _context.SaveChangesAsync();

            if (resultado < 1) return false;

            return true;
        }
    }
}
