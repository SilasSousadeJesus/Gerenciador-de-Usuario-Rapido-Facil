using Gerenciado_de_Usuario_Papido_Facil.Application.Hub;
using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Extensions;
using Gerenciado_de_Usuario_Papido_Facil.CrossCutting.Util.Enum;
using Gerenciado_de_Usuario_Papido_Facil.Domain.Entities;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class NotificacaoAppService : INotificacaoAppService
    {
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly IHubContext<NotificacaoUsuariosHub> _hubContext;
        private readonly IRetornoAppService _retornoAppService;
        private readonly IConfiguration _configuration;

        public NotificacaoAppService(IHubContext<NotificacaoUsuariosHub> hubContext, 
            INotificacaoRepository notificacaoRepository, 
            IRetornoAppService retornoAppService, 
            IConfiguration configuration)
        {
            _hubContext = hubContext;
            _notificacaoRepository = notificacaoRepository;
            _retornoAppService = retornoAppService;
            _configuration = configuration;
        }

        public async Task<RetornoGenerico> GestaoNotificacao(List<Guid> listaIds, ENotificacao tipoNotificacao, Guid idRelacionado)
        {
            var mensagem = tipoNotificacao switch
            {
                ENotificacao.cotacao => _configuration.GetSection("MensagemNotificacao")["novacotacao"],
                ENotificacao.atualizacao_cotacao => _configuration.GetSection("MensagemNotificacao")["atualizacaocotacao"],
                ENotificacao.proposta_recebida => _configuration.GetSection("MensagemNotificacao")["propostarecebida"],
                ENotificacao.proposta_aceita => _configuration.GetSection("MensagemNotificacao")["propostaaceita"],
                ENotificacao.ordem_servico_gerada => _configuration.GetSection("MensagemNotificacao")["ordem_servico_gerada"],
                ENotificacao.nova_mensagem_chat => _configuration.GetSection("MensagemNotificacao")["nova_mensagem_chat"],
                _ => null
            };

            var mensagemNotificacaoSection = _configuration.GetSection("MensagemNotificacao");

            var metodo = tipoNotificacao switch
            {
                ENotificacao.cotacao => "novacotacao",
                ENotificacao.atualizacao_cotacao => "atualizacaocotacao",
                ENotificacao.proposta_recebida => "propostarecebida",
                ENotificacao.proposta_aceita => "propostaaceita",
                ENotificacao.ordem_servico_gerada => "ordem_servico_gerada",
                ENotificacao.nova_mensagem_chat => "nova_mensagem_chat",
                _ => null
            };

            if (string.IsNullOrEmpty(mensagem) || string.IsNullOrEmpty(metodo))
                return new RetornoGenerico(false,
                                           "Notificação não enviada pois mensagem ou metodo para notificação não foi fornecida",
                                           "Notificação não enviada pois mensagem ou metodo para notificação não foi fornecida",
                                           HttpStatusCode.BadRequest);

            var resultado = await NotificarUsuariosAsync(listaIds, metodo, mensagem, tipoNotificacao, idRelacionado);

            return new RetornoGenerico(true, "Notificação enviada com sucesso", "Notificação enviada com sucesso", HttpStatusCode.OK);
        }

        public async Task<RetornoGenerico> GestaoNotificacao(Guid usuarioId, ENotificacao tipoNotificacao, Guid idRelacionado)
        {
            var mensagem = tipoNotificacao switch
            {
                ENotificacao.cotacao => _configuration.GetSection("MensagemNotificacao")["novacotacao"],
                ENotificacao.atualizacao_cotacao => _configuration.GetSection("MensagemNotificacao")["atualizacaocotacao"],
                ENotificacao.proposta_recebida => _configuration.GetSection("MensagemNotificacao")["propostarecebida"],
                ENotificacao.proposta_aceita => _configuration.GetSection("MensagemNotificacao")["propostaaceita"],
                ENotificacao.ordem_servico_gerada => _configuration.GetSection("MensagemNotificacao")["ordem_servico_gerada"],
                ENotificacao.nova_mensagem_chat => _configuration.GetSection("MensagemNotificacao")["nova_mensagem_chat"],
                _ => null
            };

            var mensagemNotificacaoSection = _configuration.GetSection("MensagemNotificacao");

            var metodo = tipoNotificacao switch
            {
                ENotificacao.cotacao => "novacotacao",
                ENotificacao.atualizacao_cotacao => "atualizacaocotacao",
                ENotificacao.proposta_recebida => "propostarecebida",
                ENotificacao.proposta_aceita => "propostaaceita",
                ENotificacao.ordem_servico_gerada => "ordem_servico_gerada",
                ENotificacao.nova_mensagem_chat => "nova_mensagem_chat",
                _ => null
            };

            if (string.IsNullOrEmpty(mensagem) || string.IsNullOrEmpty(metodo))
                return new RetornoGenerico(false,
                                           "Notificação não enviada pois mensagem ou metodo para notificação não foi fornecida",
                                           "Notificação não enviada pois mensagem ou metodo para notificação não foi fornecida",
                                           HttpStatusCode.BadRequest);

            var resultado = await NotificarUsuarioAsync(usuarioId, metodo, mensagem, tipoNotificacao, idRelacionado);

            return new RetornoGenerico(resultado.Sucesso, resultado.MensagemSistema, resultado.MensagemUsuario, resultado.HttpStatusCode);
        }

        public async Task<RetornoGenerico> NotificarUsuarioAsync(Guid userId, string metodo, string message, ENotificacao eNotificacao, Guid idRelacionado)
        {
            try
            {
                var notificacao = new Notificacao(message, userId, eNotificacao, idRelacionado);

                var resultado = await _notificacaoRepository.CriarNotificacaoAsync(notificacao);

                if (!resultado)
                {
                    return _retornoAppService.CriarRetorno(false, HttpStatusCode.InternalServerError,
                        "Não foi possível criar a notificação",
                        "Não foi possível criar a notificação");
                }

                await _hubContext.Clients.Group(userId.ToString()).SendAsync(metodo, message);

                return _retornoAppService.CriarRetorno(true, HttpStatusCode.OK, "Usuário notificado", "Usuário notificado");
            }
            catch (Exception ex)
            {
                // Opcional: logar o erro ou lidar com exceções específicas
                return _retornoAppService.CriarRetorno(false, HttpStatusCode.InternalServerError,
                    $"Erro ao notificar usuário: {ex.Message}",
                    "Erro ao processar a solicitação");
            }
        }
      
        // AJUSTAR PARA MANDAR TBM OI TIPO E MANDAR MSGH PARA TODOS OS USUARIOS COMO FEITO EM NOTIFICAR SUSUARIO
        public async Task<RetornoGenerico> NotificarUsuariosAsync(List<Guid> userIds, string metodo, string message, ENotificacao eNotificacao, Guid idRelacionado)
        {
            try
            {
                if (userIds == null || !userIds.Any())
                {
                    return _retornoAppService.CriarRetorno(false, HttpStatusCode.BadRequest,
                        "A lista de usuários está vazia ou é nula",
                        "Nenhum usuário foi informado para notificação");
                }

                var lista = userIds.Select(userId => new Notificacao(message, userId, eNotificacao, idRelacionado)).ToList();

                var resultado = await _notificacaoRepository.CriarNotificacaoAsync(lista);

                if (!resultado)
                {
                    return _retornoAppService.CriarRetorno(false, HttpStatusCode.InternalServerError,
                        "Não foi possível criar notificações para os usuários",
                        "Erro ao tentar notificar os usuários");
                }

                var tasks = userIds.Select(userId =>
                    _hubContext.Clients.Group(userId.ToString()).SendAsync(metodo, message));
                await Task.WhenAll(tasks);

                return _retornoAppService.CriarRetorno(true, HttpStatusCode.OK,
                    "Usuários notificados com sucesso",
                    "Notificações enviadas para os usuários");
            }
            catch (Exception ex)
            {
                // Opcional: logar o erro para análise
                return _retornoAppService.CriarRetorno(false, HttpStatusCode.InternalServerError,
                    $"Erro ao notificar usuários: {ex.Message}",
                    "Erro ao processar as notificações");
            }
        }
       
        public async Task<RetornoGenerico> NotificarTodosUsuariosAsync(string message, ETipoUsuario eTipoUsuario)
        {
            try
            {
                var resultado = await _notificacaoRepository.NotificarBaseUsuarioAsync(message, eTipoUsuario);

                if (!resultado)
                {
                    return _retornoAppService.CriarRetorno(false, HttpStatusCode.InternalServerError,
                        "Não foi possível notificar a base de usuários",
                        "Erro ao tentar enviar notificações para todos os usuários");
                }

                await _hubContext.Clients.All.SendAsync("novanotificacao", message);

                return _retornoAppService.CriarRetorno(true, HttpStatusCode.OK,
                    "Todos os usuários foram notificados com sucesso",
                    "Notificações enviadas para todos os usuários");
            }
            catch (Exception ex)
            {
                return _retornoAppService.CriarRetorno(false, HttpStatusCode.InternalServerError,
                    $"Erro ao notificar todos os usuários: {ex.Message}",
                    "Erro ao processar a solicitação de notificação");
            }
        }

        public async Task<RetornoGenerico> BuscarTodasNotificacoesAsync(Guid usuarioId)
        {
            var notificaoces = await _notificacaoRepository.BuscarTodasNotificacoesAsync(usuarioId);

            var resultado = notificaoces.Any();

            return new RetornoGenerico(true, resultado ? "notificações encontradas" : "não há notificações encontradas", HttpStatusCode.OK, notificaoces);
        }

        public async Task<RetornoGenerico> BuscarNotificacaoAsync(Guid usuarioId)
        {
            var notificaocao = await _notificacaoRepository.BuscarNotificacaoAsync(usuarioId);

            var resultado = notificaocao is null ? false : true;

            return new RetornoGenerico(resultado, resultado ? "notificação encontrada" : "notificação não encontrada",
                                resultado ? HttpStatusCode.OK : HttpStatusCode.NotFound, notificaocao);
        }

        public async Task<RetornoGenerico> MarcarComoLidoNotificacaoAsync(Guid notificaoId)
        {
            var resultadoNotificacao = await BuscarNotificacaoAsync(notificaoId);

            if (resultadoNotificacao.Dados is null)
            {
                return new RetornoGenerico
                {
                    Sucesso = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    MensagemSistema = "notificacao não encontrada",
                    MensagemUsuario = "notificacao não encontrada",
                    Dados = null
                };
            }

            Notificacao notificacao = resultadoNotificacao.Dados;

            notificacao.MarcarComoLido();

            await _notificacaoRepository.EditarNotificacaoAsync(notificacao);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "notificação marcada como lida",
                MensagemUsuario = "notificação marcada como lida",
                Dados = null
            };
        }

        public async Task<RetornoGenerico> MarcarTodosComoLidoNotificacaoAsync(Guid usuarioId)
        {
            await _notificacaoRepository.EditarTodasNotificacaoAsync(usuarioId);

            return new RetornoGenerico
            {
                Sucesso = true,
                HttpStatusCode = HttpStatusCode.OK,
                MensagemSistema = "todas as notificações forma marcadas como lida",
                MensagemUsuario = "todas as notificações forma marcadas como lida",
                Dados = null
            };
        }

        public Task<RetornoGenerico> NotificarUsuarioAsync(Guid userId, string metodo, string message, Guid idRelacionado)
        {
            throw new NotImplementedException();
        }

        public Task<RetornoGenerico> NotificarTodosUsuariosAsync(string message, ETipoUsuario eTipoUsuario, ENotificacao eNotificacao)
        {
            throw new NotImplementedException();
        }
    }
}
