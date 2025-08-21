using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_JobLevels_Backref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_job_roles_nome_department",
                table: "job_roles");

            migrationBuilder.DropIndex(
                name: "UX_job_levels_nome",
                table: "job_levels");

            migrationBuilder.DropIndex(
                name: "UX_departments_nome",
                table: "departments");

            migrationBuilder.RenameIndex(
                name: "UX_departments_nome1",
                table: "departments",
                newName: "UX_departments_nome");

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "job_roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "job_levels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "departments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "branches",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branches", x => x.id);
                    table.ForeignKey(
                        name: "FK_branches_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_branches_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UX_job_roles_company_dept_nome",
                table: "job_roles",
                columns: new[] { "company_id", "department_id", "nome" });

            migrationBuilder.CreateIndex(
                name: "UX_job_levels_company_nome",
                table: "job_levels",
                columns: new[] { "company_id", "nome" });

            migrationBuilder.CreateIndex(
                name: "UX_departments_company_nome",
                table: "departments",
                columns: new[] { "company_id", "nome" });

            migrationBuilder.CreateIndex(
                name: "IX_branches_address_id",
                table: "branches",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "UX_branches_company_nome",
                table: "branches",
                columns: new[] { "company_id", "nome" });

            migrationBuilder.AddForeignKey(
                name: "FK_departments_companies_company_id",
                table: "departments",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_levels_companies_company_id",
                table: "job_levels",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_job_roles_companies_company_id",
                table: "job_roles",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departments_companies_company_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "FK_job_levels_companies_company_id",
                table: "job_levels");

            migrationBuilder.DropForeignKey(
                name: "FK_job_roles_companies_company_id",
                table: "job_roles");

            migrationBuilder.DropTable(
                name: "branches");

            migrationBuilder.DropIndex(
                name: "UX_job_roles_company_dept_nome",
                table: "job_roles");

            migrationBuilder.DropIndex(
                name: "UX_job_levels_company_nome",
                table: "job_levels");

            migrationBuilder.DropIndex(
                name: "UX_departments_company_nome",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "job_roles");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "job_levels");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "companies");

            migrationBuilder.RenameIndex(
                name: "UX_departments_nome",
                table: "departments",
                newName: "UX_departments_nome1");

            migrationBuilder.CreateIndex(
                name: "UX_job_roles_nome_department",
                table: "job_roles",
                columns: new[] { "nome", "department_id" });

            migrationBuilder.CreateIndex(
                name: "UX_job_levels_nome",
                table: "job_levels",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_departments_nome",
                table: "departments",
                column: "nome");
        }
    }
}
