using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "companies");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "address_id",
                table: "companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    district = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    city = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    state = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    zipcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    complement = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "benefit_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefit_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_levels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_levels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sobrenome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    sexo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    telefone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    cargo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    departamento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    funcao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.id);
                    table.ForeignKey(
                        name: "FK_employees_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employees_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "benefit_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_benefit_types", x => x.id);
                    table.ForeignKey(
                        name: "FK_benefit_types_benefit_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "benefit_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    department_id = table.Column<int>(type: "int", nullable: false),
                    level_id = table.Column<int>(type: "int", nullable: false),
                    salario_base = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    salario_maximo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    requisitos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    responsabilidades = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_roles_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_job_roles_job_levels_level_id",
                        column: x => x.level_id,
                        principalTable: "job_levels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee_contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    grau_parentesco = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_contacts_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_contracts",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    tipo_contrato = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    jornada_horas = table.Column<int>(type: "int", nullable: true),
                    admissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    experiencia_1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    experiencia_2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    salario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    hora_entrada = table.Column<TimeSpan>(type: "time", nullable: true),
                    hora_saida = table.Column<TimeSpan>(type: "time", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_contracts", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employee_contracts_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_dependents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    grau_parentesco = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_dependents", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_dependents_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_documents",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    rg = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    orgao_emissor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    rg_emissao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    pis = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    titulo_eleitor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    zona = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    sessao = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ctps_numero = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ctps_serie = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_documents", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employee_documents_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_benefits",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    benefit_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_benefits", x => new { x.employee_id, x.benefit_type_id });
                    table.ForeignKey(
                        name: "FK_employee_benefits_benefit_types_benefit_type_id",
                        column: x => x.benefit_type_id,
                        principalTable: "benefit_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_benefits_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "job_levels",
                columns: new[] { "id", "created_at", "nome", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(5508), "Estagiário", null },
                    { 2, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6197), "Júnior", null },
                    { 3, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6271), "Pleno", null },
                    { 4, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6272), "Sênior", null },
                    { 5, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6272), "Trainer", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_companies_address_id",
                table: "companies",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_cep",
                table: "addresses",
                column: "zipcode");

            migrationBuilder.CreateIndex(
                name: "IX_benefit_types_category_id",
                table: "benefit_types",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "UX_departments_nome",
                table: "departments",
                column: "nome");

            migrationBuilder.CreateIndex(
                name: "UX_departments_nome1",
                table: "departments",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_benefits_benefit_type_id",
                table: "employee_benefits",
                column: "benefit_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_contacts_employee_id",
                table: "employee_contacts",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_dependents_employee_id",
                table: "employee_dependents",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_documents_cpf_unique",
                table: "employee_documents",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_address_id",
                table: "employees",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_company_id",
                table: "employees",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_email_unique",
                table: "employees",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_job_levels_nome",
                table: "job_levels",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_roles_department_id",
                table: "job_roles",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_job_roles_level_id",
                table: "job_roles",
                column: "level_id");

            migrationBuilder.CreateIndex(
                name: "UX_job_roles_nome_department",
                table: "job_roles",
                columns: new[] { "nome", "department_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_companies_addresses_address_id",
                table: "companies",
                column: "address_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_companies_addresses_address_id",
                table: "companies");

            migrationBuilder.DropTable(
                name: "employee_benefits");

            migrationBuilder.DropTable(
                name: "employee_contacts");

            migrationBuilder.DropTable(
                name: "employee_contracts");

            migrationBuilder.DropTable(
                name: "employee_dependents");

            migrationBuilder.DropTable(
                name: "employee_documents");

            migrationBuilder.DropTable(
                name: "job_roles");

            migrationBuilder.DropTable(
                name: "benefit_types");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "job_levels");

            migrationBuilder.DropTable(
                name: "benefit_categories");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropIndex(
                name: "IX_companies_address_id",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "companies");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
