using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciado_de_Usuario_Rapido_Facil.Infra.Migrations
{
    /// <inheritdoc />
    public partial class camposEndereco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "EmpresaPrestadora",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NumeroEndereco",
                table: "EmpresaPrestadora",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PontoReferencia",
                table: "EmpresaPrestadora",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Condomino",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NumeroEndereco",
                table: "Condomino",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PontoReferencia",
                table: "Condomino",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Condominio",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NumeroEndereco",
                table: "Condominio",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PontoReferencia",
                table: "Condominio",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "EmpresaPrestadora");

            migrationBuilder.DropColumn(
                name: "NumeroEndereco",
                table: "EmpresaPrestadora");

            migrationBuilder.DropColumn(
                name: "PontoReferencia",
                table: "EmpresaPrestadora");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Condomino");

            migrationBuilder.DropColumn(
                name: "NumeroEndereco",
                table: "Condomino");

            migrationBuilder.DropColumn(
                name: "PontoReferencia",
                table: "Condomino");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Condominio");

            migrationBuilder.DropColumn(
                name: "NumeroEndereco",
                table: "Condominio");

            migrationBuilder.DropColumn(
                name: "PontoReferencia",
                table: "Condominio");
        }
    }
}
