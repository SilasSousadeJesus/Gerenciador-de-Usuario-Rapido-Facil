using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;

namespace Gerenciado_de_Usuario_Rapido_Facil.Application.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        // Dicionário para armazenar grupos de chats criados
        //public static readonly ConcurrentDictionary<string, HashSet<string>> Grupos = new();
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            if (httpContext != null && httpContext.Request.Query.ContainsKey("access_token"))
            {
                var token = httpContext.Request.Query["access_token"];

                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(token);

                var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id");

                if (idClaim != null)
                {
                    var userId = idClaim.Value;

                    await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                }
                else
                {
                    Console.WriteLine("Claim 'Id' não encontrada no token.");
                }
            }
            else
            {
                Console.WriteLine("Token JWT não encontrado.");
            }

            await base.OnConnectedAsync();
        }

        // Método para criar um grupo de chat para dois usuários
        //public async Task CriarGrupoChatAsync(string userId1, string userId2, string groupName)
        //{
        //    if (Grupos.ContainsKey(groupName))
        //    {
        //        // O grupo já existe, então apenas notifica os usuários
        //        await Clients.Caller.SendAsync("GrupoJaExiste", groupName, "Este grupo já foi criado.");
        //        return;
        //    }

        //    // Adiciona os usuários ao grupo
        //    var usuarios = new HashSet<string> { userId1, userId2 };
        //    if (Grupos.TryAdd(groupName, usuarios))
        //    {
        //        // Adiciona os usuários ao grupo SignalR
        //        foreach (var userId in usuarios)
        //        {
        //            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //        }

        //        // Notifica que o grupo foi criado
        //        await Clients.Group(groupName).SendAsync("GrupoCriado", groupName, $"O grupo '{groupName}' foi criado para {userId1} e {userId2}");
        //    }
        //    else
        //    {
        //        // Em caso de falha ao adicionar ao dicionário
        //        await Clients.Caller.SendAsync("ErroGrupo", groupName, "Erro ao criar o grupo. Tente novamente.");
        //    }
        //}

        //// Método para enviar mensagens dentro de um grupo
        //public async Task EnviarMensagemGrupoAsync(string groupName, string message)
        //{
        //    if (!Grupos.ContainsKey(groupName))
        //    {
        //        await Clients.Caller.SendAsync("GrupoNaoExiste", groupName, "O grupo não existe.");
        //        return;
        //    }

        //    await Clients.Group(groupName).SendAsync("ReceberMensagem", message);
        //}
    }
}
