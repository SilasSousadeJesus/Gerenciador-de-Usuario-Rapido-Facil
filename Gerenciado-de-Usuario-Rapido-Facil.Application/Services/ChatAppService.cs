using AutoMapper;
using Gerenciado_de_Usuario_Rapido_Facil.Application.DTOs;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Hub;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.Application.ViewModel;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Rapido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Net;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Services
{
    public class ChatAppService : IChatAppService
    {
        private readonly IMapper _mapper;
        private readonly ICondominioRepository _condominioRepository;
        private readonly ICondominoRepository _condominoRepository;
        private readonly IValidacaoParaCadastroRepository _validacaoParaCadastroUsuario;
        private readonly IHubContext<ChatHub> _ChatHub;
        private readonly IHubContext<NotificacaoUsuariosHub> _NotificacaoHub;
        private readonly IChatRepository _chatRepository;
        private readonly INotificacaoAppService _notificacaoAppService;
        public ChatAppService(
                ICondominioRepository condominioRepository,
                IMapper mapper,
                IValidacaoParaCadastroRepository validacaoParaCadastroRepository,
                IHubContext<ChatHub> hubChatHubContext,
                IChatRepository chatRepository,
                ICondominoRepository condominoRepository,
                IHubContext<NotificacaoUsuariosHub> hubNotificacaoUsuariosHubContext,
                INotificacaoAppService notificacaoAppService
            )
        {
            _mapper = mapper;
            _condominioRepository = condominioRepository;
            _validacaoParaCadastroUsuario = validacaoParaCadastroRepository;
            _ChatHub = hubChatHubContext;
            _chatRepository = chatRepository;
            _condominoRepository = condominoRepository;
            _NotificacaoHub = hubNotificacaoUsuariosHubContext;
            _notificacaoAppService = notificacaoAppService;
        }
      
        public async Task<RetornoGenerico> BuscarChat(Guid chatId)
        {
           var chat = await _chatRepository.BuscarChat(chatId);

            var retornoChat = chat is null ? false : true;

            return new RetornoGenerico(retornoChat, retornoChat ? "chat encontrado" : "chat não encontrado",
                                retornoChat ? HttpStatusCode.OK : HttpStatusCode.NotFound, chat);
        }

        public async Task<RetornoGenerico> BuscarTodasMensagensChat(Guid chatId)
        {
            var mensagens = await _chatRepository.BuscarTodasMensagensChat(chatId);

            var retornoChat = mensagens.Any() ? true : false;

            return new RetornoGenerico(true, retornoChat ? "mensagens do chat encontrada" : "não há mensagens para este chat",
                                retornoChat ? HttpStatusCode.OK : HttpStatusCode.OK, mensagens);
        }

        public async Task<RetornoGenerico> CriarChat(CriarChatDTO chat)
        {
            var chatNovo = _mapper.Map<Chat>(chat);
            var validando = chatNovo.ValidandoSolicitante();

            if (!validando)
            {
                return new RetornoGenerico(false, "É preciso ter um solicitante", HttpStatusCode.BadRequest);
            }

            var tipoUsuario = chatNovo.IdentificandoSolicitante();

            // Busca o usuário solicitante
            dynamic usuario = tipoUsuario switch
            {
                ETipoUsuario.Condominio => await _condominioRepository.BuscarUmCondominioAsync((Guid)chatNovo.CondominioId),
                ETipoUsuario.Condomino => await _condominoRepository.BuscarUmCondominoAsync((Guid)chatNovo.CondominoId),
                _ => null
            };

            if (usuario is null)
                return new RetornoGenerico(false, "Usuário solicitante não identificado", HttpStatusCode.BadRequest);

            // Verifica se já existe um chat
            Chat buscaChat = tipoUsuario switch
            {
                ETipoUsuario.Condominio => await _chatRepository.BuscarPorParticipantes((Guid)chatNovo.CondominioId, chatNovo.EmpresaPrestadoraId, tipoUsuario),
                ETipoUsuario.Condomino => await _chatRepository.BuscarPorParticipantes((Guid)chatNovo.CondominoId, chatNovo.EmpresaPrestadoraId, tipoUsuario),
                _ => null
            };

            if (buscaChat != null)
            {
                // CONVERSÃO AQUI: Mapeia o chat existente para o ViewModel antes de retornar
                var chatExistenteViewModel = _mapper.Map<ChatRespostaViewModel>(buscaChat);

                return new RetornoGenerico(true, "Já existe um chat conjunto para esses usuários",
                                          HttpStatusCode.OK, chatExistenteViewModel);
            }

            // Cria o novo chat
            var resultado = await _chatRepository.CriarChat(chatNovo);

            if (resultado == null)
            {
                return new RetornoGenerico(false, "Não foi possível criar o chat", HttpStatusCode.InternalServerError);
            }

            // CONVERSÃO AQUI: Mapeia o chat recém-criado para o ViewModel
            var chatCriadoViewModel = _mapper.Map<ChatRespostaViewModel>(resultado);

            return new RetornoGenerico(true, "Chat criado com sucesso", HttpStatusCode.Created, chatCriadoViewModel);
        }

        public async Task<RetornoGenerico> SalvarMensagemChat(CriarMensagemChatDTO mensagemChat)
        {
            var respostaBuscaChat = await BuscarChat(mensagemChat.ChatId);

            if (!respostaBuscaChat.Sucesso) return respostaBuscaChat;

            Chat chat = respostaBuscaChat.Dados;

            List<Guid> ids = new List<Guid> { chat.EmpresaPrestadoraId};

            if (chat.CondominioId != null) ids.Add((Guid)chat.CondominioId);
            if (chat.CondominoId != null) ids.Add((Guid)chat.CondominoId);

            var novaMensagem = _mapper.Map<MensagemChat>(mensagemChat);

            var resultado = await _chatRepository.SalvarMensagemChat(novaMensagem);

            var destinatarioId = ids.Where(x => x != mensagemChat.UsuarioId).FirstOrDefault();

            await EnviarMensagemAsync(ids, destinatarioId, novaMensagem.Mensagem);

            return new RetornoGenerico(resultado, resultado ? "mensagem salva com sucesso" : "a mensagem não foi  salva",
                                resultado ? HttpStatusCode.OK : HttpStatusCode.InternalServerError);
        }

        public async Task EnviarMensagemAsync(IEnumerable<Guid> userIds, Guid destinatarioId, string message)
        {

            if (!userIds.Any()) return;

            var tasks = userIds.Select(userId =>
                                _ChatHub.Clients.Group(userId.ToString()).SendAsync("novamensagem", message));

            await Task.WhenAll(tasks);

            await _notificacaoAppService.GestaoNotificacao(destinatarioId, ENotificacao.nova_mensagem_chat, Guid.Empty);
        }


    }
}
