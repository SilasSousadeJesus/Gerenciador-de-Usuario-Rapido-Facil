using Gerenciado_de_Usuario_Rapido_Facil.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.config.configMigrate
{
    public static  class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var GerenciadorUsuarioContext = services.GetRequiredService<GerenciadorUsuarioDbContext>();
                    GerenciadorUsuarioContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
                    throw;
                }
            }
            return app;
        }
    }
}
