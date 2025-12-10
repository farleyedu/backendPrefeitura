using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalTCMSP.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Atualizando_migration_noticia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoticiaOld");

            migrationBuilder.AlterColumn<int>(
                name: "versao",
                table: "noticias",
                type: "int",
                nullable: true,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "noticias",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "atualizado_em",
                table: "noticias",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "versao",
                table: "noticias",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "noticias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "atualizado_em",
                table: "noticias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NoticiaOld",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Atualizado_Em = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Atualizado_Por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor_Creditos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor_Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Canonical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: true),
                    CategoriasExtras = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConteudoNoticia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Criado_Em = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Criado_Por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Destaque = table.Column<bool>(type: "bit", nullable: false),
                    ImagemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Og_Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resumo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seo_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seo_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtitulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Versao = table.Column<int>(type: "int", nullable: true),
                    Visualizacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticiaOld", x => x.Id);
                });
        }
    }
}
