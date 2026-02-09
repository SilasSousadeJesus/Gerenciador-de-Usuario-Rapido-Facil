using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateCopiaServicoSubtipoParaCotacaoProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE IF NOT EXISTS CopiaServicoSubtipoParaCotacao()
            BEGIN
                -- Copiar Servicos
                INSERT INTO cotacaorapidofacil.servicos (Id, Nome)
                SELECT s.Id, s.Nome
                FROM gerenciadorusuario.servicos s
                WHERE NOT EXISTS (
                    SELECT 1 FROM cotacaorapidofacil.servicos cs WHERE cs.Id = s.Id
                );
                
                -- Copiar ServicosSubtipos
                INSERT INTO cotacaorapidofacil.servicoSubtipos (Id, Nome, ServicoId)
                SELECT st.Id, st.Nome, st.ServicoId
                FROM gerenciadorusuario.servicoSubtipos st
                WHERE NOT EXISTS (
                    SELECT 1 FROM cotacaorapidofacil.servicoSubtipos cst WHERE cst.Id = st.Id
                );
            END;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CopiaServicoSubtipoParaCotacao;");
        }
    }
}