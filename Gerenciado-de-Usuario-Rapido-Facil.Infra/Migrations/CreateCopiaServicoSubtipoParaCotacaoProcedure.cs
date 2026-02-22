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
                        INSERT INTO `cotacaorapidofacil`.`Servicos` (Id, Nome)
                        SELECT s.Id, s.Nome
                        FROM `gerenciadorusuario`.`Servicos` s
                        WHERE NOT EXISTS (
                            SELECT 1 FROM `cotacaorapidofacil`.`Servicos` cs WHERE cs.Id = s.Id
                        );

                        INSERT INTO `cotacaorapidofacil`.`ServicoSubtipos` (Id, Nome, ServicoId)
                        SELECT st.Id, st.Nome, st.ServicoId
                        FROM `gerenciadorusuario`.`ServicoSubtipos` st
                        WHERE NOT EXISTS (
                            SELECT 1 FROM `cotacaorapidofacil`.`ServicoSubtipos` cst WHERE cst.Id = st.Id
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