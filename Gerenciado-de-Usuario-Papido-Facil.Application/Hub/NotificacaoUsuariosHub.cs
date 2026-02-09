
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Hub
{
    public class NotificacaoUsuariosHub : Microsoft.AspNetCore.SignalR.Hub
    {
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

                    //Context.Items["UserId"] = userId;

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
    }
}
