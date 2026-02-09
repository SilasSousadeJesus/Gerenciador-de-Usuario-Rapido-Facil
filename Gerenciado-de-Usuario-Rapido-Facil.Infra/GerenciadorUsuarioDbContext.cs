
using Gerenciado_de_Usuario_Rapido_Facil.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra
{
    public class GerenciadorUsuarioDbContext : DbContext
    {
        public GerenciadorUsuarioDbContext(DbContextOptions<GerenciadorUsuarioDbContext> options)
        : base(options)
        {
        }

        public DbSet<Condominio> Condominio { get; set; }
        public DbSet<Condomino> Condomino { get; set; }
        public DbSet<EmpresaPrestadora> EmpresaPrestadora { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<ServicoSubtipo> ServicoSubtipos { get; set; }
        public DbSet<EmpresaPrestadoraServicoSubtipo> EmpresaPrestadoraServicos { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<MensagemChat> MensagemChat { get; set; }
        public DbSet<Notificacao> Notificacao { get; set; }
        public DbSet<Idoneidade> Idoneidade { get; set; }
        public DbSet<CND> CND { get; set; }
        public DbSet<CNDT> CNDT { get; set; }
        public DbSet<TemplateHTML> TemplateHTML { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Força o nome da tabela = nome da classe (case-sensitive safe para MySQL Linux)

            modelBuilder.Entity<TemplateHTML>()
                .ToTable("TemplateHTML"); // 👈 OBRIGATÓRIO

            // Configurando a chave composta de EmpresaPrestadoraServicoSubtipo
            modelBuilder.Entity<EmpresaPrestadoraServicoSubtipo>()
                .HasKey(epss => new { epss.EmpresaPrestadoraId, epss.ServicoId, epss.ServicoSubtipoId });

            // Configurando o relacionamento entre EmpresaPrestadora e EmpresaPrestadoraServicoSubtipo
            modelBuilder.Entity<EmpresaPrestadoraServicoSubtipo>()
                .HasOne(epss => epss.EmpresaPrestadora)
                .WithMany(ep => ep.EmpresaPrestadoraServicoSubtipos)
                .HasForeignKey(epss => epss.EmpresaPrestadoraId);

            // Configurando o relacionamento entre Servico e EmpresaPrestadoraServicoSubtipo
            modelBuilder.Entity<EmpresaPrestadoraServicoSubtipo>()
                .HasOne(epss => epss.Servico)
                .WithMany(s => s.EmpresaPrestadoraServicos)
                .HasForeignKey(epss => epss.ServicoId);

                // Configurando o relacionamento entre ServicoSubtipo e EmpresaPrestadoraServicoSubtipo
                modelBuilder.Entity<EmpresaPrestadoraServicoSubtipo>()
                    .HasOne(epss => epss.ServicoSubtipo)
                    .WithMany(ss => ss.EmpresaPrestadoraServicoSubtipos)
                    .HasForeignKey(epss => epss.ServicoSubtipoId);

                modelBuilder.Entity<Condominio>()
                    .HasIndex(u => u.Email)
                    .IsUnique();

                modelBuilder.Entity<Condomino>()
                         .HasIndex(u => u.Email)
                         .IsUnique();

                modelBuilder.Entity<EmpresaPrestadora>()
                 .HasIndex(u => u.Email)
                 .IsUnique();
        }


        public void ProcedureCopiarServicoSubtipo()
        {
            this.Database.ExecuteSqlRaw("CALL CopiaServicoSubtipoParaCotacao()");
        }
    }
}
