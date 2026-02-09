using Gerenciado_de_Usuario_Papido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Papido_Facil.Infra.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Gerenciado_de_Usuario_Papido_Facil.Application.Services
{
    public class EmailAppService  : IEmailAppService
    {
        private readonly IConfiguration _configuration;
        private readonly ITemplateHTMLRepository _templateHTMLRepository;

        public EmailAppService(IConfiguration configuration, ITemplateHTMLRepository    templateHTMLRepository)
        {
            _configuration = configuration;
            _templateHTMLRepository = templateHTMLRepository;

        }

        public async Task EnviarEmailVinculo(string destinatario, string assunto, string body) {
           
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using var client = new SmtpClient
                {
                    Host = "live.smtp.mailtrap.io",
                    Port = int.Parse(smtpSettings["Port"]),
                    EnableSsl = true,
                    Credentials = new NetworkCredential("api", "251c845d3249668a5aff7a4fdf2da4b8")
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("hello@demomailtrap.com", smtpSettings["FromName"]),
                    Subject = assunto,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add("silassousadejesus@gmail.com");

                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new Exception($"Erro ao enviar email: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao enviar email: {ex.Message}", ex);
            }

        }
    }
}
