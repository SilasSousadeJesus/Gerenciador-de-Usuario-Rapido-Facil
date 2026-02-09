
using Gerenciado_de_Usuario_Rapido_Facil.Application.Configuration;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Hub;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.Application.Services;
using Gerenciado_de_Usuario_Rapido_Facil.Infra;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.config.configMigrate;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Interfaces;
using Gerenciado_de_Usuario_Rapido_Facil.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Usuario_Rapido_Facil.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<GerenciadorUsuarioDbContext>(options =>
                                                                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                                                                    new MySqlServerVersion(new Version(8, 0, 26)),
                                                                    mysqlOptions => mysqlOptions.MigrationsAssembly("Gerenciado-de-Usuario-Rapido-Facil.Infra")
                                                                    .EnableRetryOnFailure()));

            // Add services to the container.

            builder.Services.AddScoped<IBuscaInformacoesExternas, BuscaInformacoesExternas>();

            builder.Services.AddScoped<IValidacaoParaCadastroRepository, ValidacaoParaCadastroRepository>();

            builder.Services.AddScoped<ICondominioAppService, CondominioAppService>();
            builder.Services.AddScoped<ICondominioRepository, CondominioRepository>();

            builder.Services.AddScoped<ICondominoAppService, CondominoAppService>();
            builder.Services.AddScoped<ICondominoRepository, CondominoRepository>();

            builder.Services.AddScoped<IServicoAppService, ServicoAppService>();
            builder.Services.AddScoped<IServicoRepository, ServicoRepository>();

            builder.Services.AddScoped<IEmpresaPrestadoraAppService, EmpresaPrestadoraAppService>();
            builder.Services.AddScoped<IEmpresaPrestadoraRepository, EmpresaPrestadoraRepository>();

            builder.Services.AddScoped<I_IdentificacaoAppService, IdentificacaoAppService>();
            builder.Services.AddScoped<I_IdentificacaoRepository, IdentificacaoRepository>();

            builder.Services.AddScoped<IChatAppService, ChatAppService>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();

            builder.Services.AddScoped<IHttpAppService, HttpAppService>();

            builder.Services.AddScoped<INotificacaoAppService, NotificacaoAppService>();
            builder.Services.AddScoped<INotificacaoRepository, NotificacaoRepository>();

            builder.Services.AddScoped<IAnaliseIdoneidadeAppService, AnaliseIdoneidadeAppService>();
            builder.Services.AddScoped<IAnaliseIdoneidadeRepository, AnaliseIdoneidadeRepository>();

            builder.Services.AddScoped<IEmailAppService, EmailAppService>();


            builder.Services.AddScoped<ITemplateHTMLRepository, TemplateHTMLRepository>();


            builder.Services.AddScoped<IRetornoAppService, RetornoAppService>();


            builder.Services.AddHttpClient();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://rapido-facil-front-end-production.up.railway.app")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            //builder.Services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.SetIsOriginAllowed(origin => true) // Permite qualquer origem
            //               .AllowAnyMethod()
            //               .AllowAnyHeader()
            //               .AllowCredentials(); // Permite envio de credenciais
            //    });
            //});

            builder.Services.AddSignalR();

            var app = builder.Build();

            app.MigrateDatabase();



            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseCors();

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHub>("chat");

            app.MapHub<NotificacaoUsuariosHub>("notificacao");


            app.Run();
        }
    }
}


// criar logica pra ativa ou desativar condominio e refelxo disso nos condominos
// ajustar edição para atualizar os usuarios que usam codigo vinculação caso ele mude