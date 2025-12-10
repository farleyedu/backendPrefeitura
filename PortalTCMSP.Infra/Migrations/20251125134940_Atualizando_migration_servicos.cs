using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortalTCMSP.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Atualizando_migration_servicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "servicos_carta_servicos_usuario",
                columns: table => new
                {
                    id_carta_servicos_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    titulo_pesquisa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_carta_servicos_usuario", x => x.id_carta_servicos_usuario);
                });

            migrationBuilder.CreateTable(
                name: "servicos_cartorio",
                columns: table => new
                {
                    id_cartorio = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_cartorio", x => x.id_cartorio);
                });

            migrationBuilder.CreateTable(
                name: "servicos_emissao_certidoes",
                columns: table => new
                {
                    id_emissao_certidoes = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_emissao_certidoes", x => x.id_emissao_certidoes);
                });

            migrationBuilder.CreateTable(
                name: "servicos_multas_procedimentos",
                columns: table => new
                {
                    id_multas_procedimentos = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_multas_procedimentos", x => x.id_multas_procedimentos);
                });

            migrationBuilder.CreateTable(
                name: "servicos_oficiose_intimacoes",
                columns: table => new
                {
                    id_oficiose_intimacoes = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_oficiose_intimacoes", x => x.id_oficiose_intimacoes);
                });

            migrationBuilder.CreateTable(
                name: "servicos_prazos_processuais",
                columns: table => new
                {
                    id_prazos_processuais = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_prazos_processuais", x => x.id_prazos_processuais);
                });

            migrationBuilder.CreateTable(
                name: "servicos_protocolo_eletronico",
                columns: table => new
                {
                    id_protocolo_eletronico = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_protocolo_eletronico", x => x.id_protocolo_eletronico);
                });

            migrationBuilder.CreateTable(
                name: "servicos_carta_servicos_usuario_servico",
                columns: table => new
                {
                    id_carta_servicos_usuario_servico = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_carta_servicos_usuario = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_carta_servicos_usuario_servico", x => x.id_carta_servicos_usuario_servico);
                    table.ForeignKey(
                        name: "FK_servicos_carta_servicos_usuario_servico_servicos_carta_servicos_usuario_id_carta_servicos_usuario",
                        column: x => x.id_carta_servicos_usuario,
                        principalTable: "servicos_carta_servicos_usuario",
                        principalColumn: "id_carta_servicos_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_cartorio_atendimento",
                columns: table => new
                {
                    id_cartorio_atendimento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cartorio = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_cartorio_atendimento", x => x.id_cartorio_atendimento);
                    table.ForeignKey(
                        name: "fk_cartorio_atendimento__cartorio",
                        column: x => x.id_cartorio,
                        principalTable: "servicos_cartorio",
                        principalColumn: "id_cartorio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_emissao_certidoes_acao",
                columns: table => new
                {
                    id_emissao_certidoes_acao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_emissao_certidoes = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_publicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipo_acao = table.Column<int>(type: "int", nullable: false),
                    url_acao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_emissao_certidoes_acao", x => x.id_emissao_certidoes_acao);
                    table.ForeignKey(
                        name: "FK_servicos_emissao_certidoes_acao_servicos_emissao_certidoes_id_emissao_certidoes",
                        column: x => x.id_emissao_certidoes,
                        principalTable: "servicos_emissao_certidoes",
                        principalColumn: "id_emissao_certidoes",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_emissao_certidoes_secao_orientacao",
                columns: table => new
                {
                    id_emissao_certidoes_secao_orientacao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_emissao_certidoes = table.Column<long>(type: "bigint", nullable: false),
                    tipo_secao = table.Column<int>(type: "int", nullable: false),
                    titulo_pagina = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_emissao_certidoes_secao_orientacao", x => x.id_emissao_certidoes_secao_orientacao);
                    table.ForeignKey(
                        name: "FK_servicos_emissao_certidoes_secao_orientacao_servicos_emissao_certidoes_id_emissao_certidoes",
                        column: x => x.id_emissao_certidoes,
                        principalTable: "servicos_emissao_certidoes",
                        principalColumn: "id_emissao_certidoes",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_multas_procedimentos_portaria_relacionada",
                columns: table => new
                {
                    id_multas_procedimentos_portaria_relacionada = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_multas_procedimentos = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_multas_procedimentos_portaria_relacionada", x => x.id_multas_procedimentos_portaria_relacionada);
                    table.ForeignKey(
                        name: "fk_multas_procedimentos_portaria_relacionada__multas_procedimentos",
                        column: x => x.id_multas_procedimentos,
                        principalTable: "servicos_multas_procedimentos",
                        principalColumn: "id_multas_procedimentos",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_multas_procedimentos_procedimento",
                columns: table => new
                {
                    id_multas_procedimentos_procedimento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_multas_procedimentos = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url_imagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_multas_procedimentos_procedimento", x => x.id_multas_procedimentos_procedimento);
                    table.ForeignKey(
                        name: "fk_multas_procedimentos_procedimento__multas_procedimentos",
                        column: x => x.id_multas_procedimentos,
                        principalTable: "servicos_multas_procedimentos",
                        principalColumn: "id_multas_procedimentos",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_oficiose_intimacoes_secao",
                columns: table => new
                {
                    id_oficiose_intimacoes_secao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_oficiose_intimacoes = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_oficiose_intimacoes_secao", x => x.id_oficiose_intimacoes_secao);
                    table.ForeignKey(
                        name: "fk_oficiose_intimacoes_secao__oficiose_intimacoes",
                        column: x => x.id_oficiose_intimacoes,
                        principalTable: "servicos_oficiose_intimacoes",
                        principalColumn: "id_oficiose_intimacoes",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_prazos_processuais_item",
                columns: table => new
                {
                    id_prazos_processuais_item = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_prazos_processuais = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_publicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tempo_decorrido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_prazos_processuais_item", x => x.id_prazos_processuais_item);
                    table.ForeignKey(
                        name: "fk_prazos_processuais_item__prazos_processuais",
                        column: x => x.id_prazos_processuais,
                        principalTable: "servicos_prazos_processuais",
                        principalColumn: "id_prazos_processuais",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_protocolo_eletronico_acao",
                columns: table => new
                {
                    id_protocolo_eletronico_acao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_protocolo_eletronico = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_publicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipo_acao = table.Column<int>(type: "int", nullable: false),
                    url_acao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_protocolo_eletronico_acao", x => x.id_protocolo_eletronico_acao);
                    table.ForeignKey(
                        name: "fk_protocolo_eletronico_acao__protocolo_eletronico",
                        column: x => x.id_protocolo_eletronico,
                        principalTable: "servicos_protocolo_eletronico",
                        principalColumn: "id_protocolo_eletronico",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_carta_servicos_usuario_servico_item",
                columns: table => new
                {
                    id_carta_servicos_usuario_servico_item = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_carta_servicos_usuario_servico = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    acao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    link_item = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_carta_servicos_usuario_servico_item", x => x.id_carta_servicos_usuario_servico_item);
                    table.ForeignKey(
                        name: "FK_servicos_carta_servicos_usuario_servico_item_servicos_carta_servicos_usuario_servico_id_carta_servicos_usuario_servico",
                        column: x => x.id_carta_servicos_usuario_servico,
                        principalTable: "servicos_carta_servicos_usuario_servico",
                        principalColumn: "id_carta_servicos_usuario_servico",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_emissao_certidoes_orientacao",
                columns: table => new
                {
                    id_emissao_certidoes_orientacao = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_emissao_certidoes_secao_orientacao = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_emissao_certidoes_orientacao", x => x.id_emissao_certidoes_orientacao);
                    table.ForeignKey(
                        name: "FK_servicos_emissao_certidoes_orientacao_servicos_emissao_certidoes_secao_orientacao_id_emissao_certidoes_secao_orientacao",
                        column: x => x.id_emissao_certidoes_secao_orientacao,
                        principalTable: "servicos_emissao_certidoes_secao_orientacao",
                        principalColumn: "id_emissao_certidoes_secao_orientacao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_oficiose_intimacoes_secao_item",
                columns: table => new
                {
                    id_oficiose_intimacoes_secao_item = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_oficiose_intimacoes_secao = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_oficiose_intimacoes_secao_item", x => x.id_oficiose_intimacoes_secao_item);
                    table.ForeignKey(
                        name: "fk_oficiose_intimacoes_secao_item__oficiose_intimacoes_secao",
                        column: x => x.id_oficiose_intimacoes_secao,
                        principalTable: "servicos_oficiose_intimacoes_secao",
                        principalColumn: "id_oficiose_intimacoes_secao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_prazos_processuais_item_anexo",
                columns: table => new
                {
                    id_prazos_processuais_item_anexo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_prazos_processuais_item = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    nome_arquivo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_prazos_processuais_item_anexo", x => x.id_prazos_processuais_item_anexo);
                    table.ForeignKey(
                        name: "fk_prazos_processuais_item_anexo__prazos_processuais_item",
                        column: x => x.id_prazos_processuais_item,
                        principalTable: "servicos_prazos_processuais_item",
                        principalColumn: "id_prazos_processuais_item",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_carta_servicos_usuario_item_detalhe",
                columns: table => new
                {
                    id_carta_servicos_usuario_item_detalhe = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_carta_servicos_usuario_servico_item = table.Column<long>(type: "bigint", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    titulo_detalhe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_carta_servicos_usuario_item_detalhe", x => x.id_carta_servicos_usuario_item_detalhe);
                    table.ForeignKey(
                        name: "FK_servicos_carta_servicos_usuario_item_detalhe_servicos_carta_servicos_usuario_servico_item_id_carta_servicos_usuario_servico_~",
                        column: x => x.id_carta_servicos_usuario_servico_item,
                        principalTable: "servicos_carta_servicos_usuario_servico_item",
                        principalColumn: "id_carta_servicos_usuario_servico_item",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_emissao_certidoes_descritivo",
                columns: table => new
                {
                    id_emissao_certidoes_descritivo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_emissao_certidoes_orientacao = table.Column<long>(type: "bigint", nullable: false),
                    texto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_emissao_certidoes_descritivo", x => x.id_emissao_certidoes_descritivo);
                    table.ForeignKey(
                        name: "FK_servicos_emissao_certidoes_descritivo_servicos_emissao_certidoes_orientacao_id_emissao_certidoes_orientacao",
                        column: x => x.id_emissao_certidoes_orientacao,
                        principalTable: "servicos_emissao_certidoes_orientacao",
                        principalColumn: "id_emissao_certidoes_orientacao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "servicos_carta_servicos_usuario_descritivo_item_detalhe",
                columns: table => new
                {
                    id_carta_servicos_usuario_descritivo_item_detalhe = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_carta_servicos_usuario_item_detalhe = table.Column<long>(type: "bigint", nullable: false),
                    ordem = table.Column<int>(type: "int", nullable: false),
                    descritivo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicos_carta_servicos_usuario_descritivo_item_detalhe", x => x.id_carta_servicos_usuario_descritivo_item_detalhe);
                    table.ForeignKey(
                        name: "FK_servicos_carta_servicos_usuario_descritivo_item_detalhe_servicos_carta_servicos_usuario_item_detalhe_id_carta_servicos_usuar~",
                        column: x => x.id_carta_servicos_usuario_item_detalhe,
                        principalTable: "servicos_carta_servicos_usuario_item_detalhe",
                        principalColumn: "id_carta_servicos_usuario_item_detalhe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_servicos_carta_servicos_usuario_descritivo_item_detalhe_id_carta_servicos_usuario_item_detalhe",
                table: "servicos_carta_servicos_usuario_descritivo_item_detalhe",
                column: "id_carta_servicos_usuario_item_detalhe");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_carta_servicos_usuario_item_detalhe_id_carta_servicos_usuario_servico_item",
                table: "servicos_carta_servicos_usuario_item_detalhe",
                column: "id_carta_servicos_usuario_servico_item");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_carta_servicos_usuario_servico_id_carta_servicos_usuario",
                table: "servicos_carta_servicos_usuario_servico",
                column: "id_carta_servicos_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_carta_servicos_usuario_servico_item_id_carta_servicos_usuario_servico",
                table: "servicos_carta_servicos_usuario_servico_item",
                column: "id_carta_servicos_usuario_servico");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_cartorio_atendimento_id_cartorio",
                table: "servicos_cartorio_atendimento",
                column: "id_cartorio");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_emissao_certidoes_acao_id_emissao_certidoes",
                table: "servicos_emissao_certidoes_acao",
                column: "id_emissao_certidoes");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_emissao_certidoes_descritivo_id_emissao_certidoes_orientacao",
                table: "servicos_emissao_certidoes_descritivo",
                column: "id_emissao_certidoes_orientacao");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_emissao_certidoes_orientacao_id_emissao_certidoes_secao_orientacao",
                table: "servicos_emissao_certidoes_orientacao",
                column: "id_emissao_certidoes_secao_orientacao");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_emissao_certidoes_secao_orientacao_id_emissao_certidoes",
                table: "servicos_emissao_certidoes_secao_orientacao",
                column: "id_emissao_certidoes");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_multas_procedimentos_portaria_relacionada_id_multas_procedimentos",
                table: "servicos_multas_procedimentos_portaria_relacionada",
                column: "id_multas_procedimentos");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_multas_procedimentos_procedimento_id_multas_procedimentos",
                table: "servicos_multas_procedimentos_procedimento",
                column: "id_multas_procedimentos");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_oficiose_intimacoes_secao_id_oficiose_intimacoes",
                table: "servicos_oficiose_intimacoes_secao",
                column: "id_oficiose_intimacoes");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_oficiose_intimacoes_secao_item_id_oficiose_intimacoes_secao",
                table: "servicos_oficiose_intimacoes_secao_item",
                column: "id_oficiose_intimacoes_secao");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_prazos_processuais_item_id_prazos_processuais",
                table: "servicos_prazos_processuais_item",
                column: "id_prazos_processuais");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_prazos_processuais_item_anexo_id_prazos_processuais_item",
                table: "servicos_prazos_processuais_item_anexo",
                column: "id_prazos_processuais_item");

            migrationBuilder.CreateIndex(
                name: "IX_servicos_protocolo_eletronico_acao_id_protocolo_eletronico",
                table: "servicos_protocolo_eletronico_acao",
                column: "id_protocolo_eletronico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "servicos_carta_servicos_usuario_descritivo_item_detalhe");

            migrationBuilder.DropTable(
                name: "servicos_cartorio_atendimento");

            migrationBuilder.DropTable(
                name: "servicos_emissao_certidoes_acao");

            migrationBuilder.DropTable(
                name: "servicos_emissao_certidoes_descritivo");

            migrationBuilder.DropTable(
                name: "servicos_multas_procedimentos_portaria_relacionada");

            migrationBuilder.DropTable(
                name: "servicos_multas_procedimentos_procedimento");

            migrationBuilder.DropTable(
                name: "servicos_oficiose_intimacoes_secao_item");

            migrationBuilder.DropTable(
                name: "servicos_prazos_processuais_item_anexo");

            migrationBuilder.DropTable(
                name: "servicos_protocolo_eletronico_acao");

            migrationBuilder.DropTable(
                name: "servicos_carta_servicos_usuario_item_detalhe");

            migrationBuilder.DropTable(
                name: "servicos_cartorio");

            migrationBuilder.DropTable(
                name: "servicos_emissao_certidoes_orientacao");

            migrationBuilder.DropTable(
                name: "servicos_multas_procedimentos");

            migrationBuilder.DropTable(
                name: "servicos_oficiose_intimacoes_secao");

            migrationBuilder.DropTable(
                name: "servicos_prazos_processuais_item");

            migrationBuilder.DropTable(
                name: "servicos_protocolo_eletronico");

            migrationBuilder.DropTable(
                name: "servicos_carta_servicos_usuario_servico_item");

            migrationBuilder.DropTable(
                name: "servicos_emissao_certidoes_secao_orientacao");

            migrationBuilder.DropTable(
                name: "servicos_oficiose_intimacoes");

            migrationBuilder.DropTable(
                name: "servicos_prazos_processuais");

            migrationBuilder.DropTable(
                name: "servicos_carta_servicos_usuario_servico");

            migrationBuilder.DropTable(
                name: "servicos_emissao_certidoes");

            migrationBuilder.DropTable(
                name: "servicos_carta_servicos_usuario");
        }
    }
}
