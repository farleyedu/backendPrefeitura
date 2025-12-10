using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalTCMSP.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Atualizando_migration_relatorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "institucional_bloco_subtexto",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_descricao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institucional_bloco_subtexto", x => x.id);
                    table.ForeignKey(
                        name: "FK_institucional_bloco_subtexto_institucional_bloco_descricoes_id_descricao",
                        column: x => x.id_descricao,
                        principalTable: "institucional_bloco_descricoes",
                        principalColumn: "id_bloco_descricao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoticiaOld",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subtitulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resumo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriasExtras = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor_Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor_Creditos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seo_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seo_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Og_Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Canonical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Destaque = table.Column<bool>(type: "bit", nullable: false),
                    Visualizacao = table.Column<int>(type: "int", nullable: false),
                    ImagemUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConteudoNoticia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Criado_Em = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Criado_Por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atualizado_Em = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Atualizado_Por = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Versao = table.Column<int>(type: "int", nullable: true),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticiaOld", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_institucional_bloco_subtexto_id_descricao",
                table: "institucional_bloco_subtexto",
                column: "id_descricao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "institucional_bloco_subtexto");

            migrationBuilder.DropTable(
                name: "NoticiaOld");
        }
    }
}
