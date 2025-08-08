using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: true),
                    address = table.Column<string>(type: "TEXT", nullable: true),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "itens_uniformes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    code = table.Column<string>(type: "TEXT", nullable: true),
                    descricao = table.Column<string>(type: "TEXT", nullable: true),
                    tamanho = table.Column<string>(type: "TEXT", nullable: true),
                    total_estoque = table.Column<int>(type: "INTEGER", nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itens_uniformes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    password = table.Column<string>(type: "TEXT", nullable: true),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "funcionarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    company_id = table.Column<int>(type: "INTEGER", nullable: true),
                    nome = table.Column<string>(type: "TEXT", nullable: true),
                    sobrenome = table.Column<string>(type: "TEXT", nullable: true),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    telefone = table.Column<string>(type: "TEXT", nullable: true),
                    posicao = table.Column<string>(type: "TEXT", nullable: true),
                    data_de_admissao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcionarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_funcionarios_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_companies",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "INTEGER", nullable: false),
                    company_id = table.Column<int>(type: "INTEGER", nullable: false),
                    role = table.Column<string>(type: "TEXT", nullable: true),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_companies", x => new { x.user_id, x.company_id });
                    table.ForeignKey(
                        name: "FK_user_companies_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_companies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "atribuicao_uniformes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    funcionario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    itens_uniformes_id = table.Column<int>(type: "INTEGER", nullable: false),
                    quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    data_entrega = table.Column<DateTime>(type: "TEXT", nullable: false),
                    data_retornada = table.Column<DateTime>(type: "TEXT", nullable: true),
                    data_devolucao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    condicoes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_atribuicao_uniformes", x => x.id);
                    table.ForeignKey(
                        name: "FK_atribuicao_uniformes_funcionarios_funcionario_id",
                        column: x => x.funcionario_id,
                        principalTable: "funcionarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_atribuicao_uniformes_itens_uniformes_itens_uniformes_id",
                        column: x => x.itens_uniformes_id,
                        principalTable: "itens_uniformes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "folha_de_pagamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    funcionario_id = table.Column<int>(type: "INTEGER", nullable: false),
                    hora_entrada = table.Column<DateTime>(type: "TEXT", nullable: false),
                    hora_saida = table.Column<DateTime>(type: "TEXT", nullable: false),
                    salario_base = table.Column<decimal>(type: "TEXT", nullable: false),
                    bonus = table.Column<decimal>(type: "TEXT", nullable: false),
                    deducoes = table.Column<decimal>(type: "TEXT", nullable: false),
                    salario_liquido = table.Column<decimal>(type: "TEXT", nullable: false),
                    data_pagamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createdAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folha_de_pagamento", x => x.id);
                    table.ForeignKey(
                        name: "FK_folha_de_pagamento_funcionarios_funcionario_id",
                        column: x => x.funcionario_id,
                        principalTable: "funcionarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_atribuicao_uniformes_funcionario_id",
                table: "atribuicao_uniformes",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_atribuicao_uniformes_itens_uniformes_id",
                table: "atribuicao_uniformes",
                column: "itens_uniformes_id");

            migrationBuilder.CreateIndex(
                name: "IX_folha_de_pagamento_funcionario_id",
                table: "folha_de_pagamento",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_funcionarios_company_id",
                table: "funcionarios",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_companies_company_id",
                table: "user_companies",
                column: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "atribuicao_uniformes");

            migrationBuilder.DropTable(
                name: "folha_de_pagamento");

            migrationBuilder.DropTable(
                name: "user_companies");

            migrationBuilder.DropTable(
                name: "itens_uniformes");

            migrationBuilder.DropTable(
                name: "funcionarios");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
