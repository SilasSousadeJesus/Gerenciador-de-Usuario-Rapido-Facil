using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly GerenciadorUsuarioDbContext _context;

        public ChatRepository(GerenciadorUsuarioDbContext context)
        {
            _context = context;
        }

        public async Task<Chat> BuscarChat(Guid chatId)
        {
            return await _context.Set<Chat>().Where(x => x.Id == chatId).Include(x => x.mensagens).FirstOrDefaultAsync();
        }

        public async Task<Chat?> BuscarPorParticipantes(Guid solicitanteId, Guid EmpresaPrestadoraId, ETipoUsuario eTipoUsuario)
        {
            IQueryable<Chat> query;

            switch (eTipoUsuario)
            {
                case ETipoUsuario.Condominio:
                    query = _context.Set<Chat>()
                        .Where(x => x.CondominioId == solicitanteId && x.EmpresaPrestadoraId == EmpresaPrestadoraId)
                        .Include(x => x.mensagens);
                    break;

                case ETipoUsuario.Condomino:
                    query = _context.Set<Chat>()
                        .Where(x => x.CondominoId == solicitanteId && x.EmpresaPrestadoraId == EmpresaPrestadoraId)
                        .Include(x => x.mensagens);
                    break;

                default:
                    return null;
            }

            var chat = await query.FirstOrDefaultAsync();

            if (chat != null && chat.mensagens == null)
                chat.mensagens = new List<MensagemChat>();

            return chat;
        }



        public async Task<List<MensagemChat>> BuscarTodasMensagensChat(Guid chatId)
        {
            return await _context.Set<MensagemChat>().Where(x=> x.ChatId == chatId).ToListAsync() ?? []; 
        }

        public async Task<Chat?> CriarChat(Chat chat)
        {
            var entidade = await _context.Set<Chat>().AddAsync(chat);
            var resultado = await _context.SaveChangesAsync();
            var novoChat = entidade.Entity;
            return resultado < 1 ? null : novoChat;
        }

        public async Task<bool> SalvarMensagemChat(MensagemChat mensagemChat)
        {
            var entidade = await _context.Set<MensagemChat>().AddAsync(mensagemChat);
            var resultado =  await _context.SaveChangesAsync();

            if (resultado < 0) return false;    

            return true;
        }

    }
}
