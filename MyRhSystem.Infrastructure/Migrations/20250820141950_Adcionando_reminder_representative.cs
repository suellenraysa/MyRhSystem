using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adcionando_reminder_representative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cnpj",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "telefone",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "legal_representatives",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_representatives", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_representatives_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reminders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    assigned_user_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    due_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    remind_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_done = table.Column<bool>(type: "bit", nullable: false),
                    recurrence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reminders", x => x.id);
                    table.ForeignKey(
                        name: "FK_reminders_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reminders_users_assigned_user_id",
                        column: x => x.assigned_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_legal_representatives_company_id",
                table: "legal_representatives",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_reminders_assigned_user_id",
                table: "reminders",
                column: "assigned_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reminders_company_id_is_done",
                table: "reminders",
                columns: new[] { "company_id", "is_done" });

            migrationBuilder.CreateIndex(
                name: "IX_reminders_due_at",
                table: "reminders",
                column: "due_at");

            migrationBuilder.CreateIndex(
                name: "IX_reminders_remind_at",
                table: "reminders",
                column: "remind_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "legal_representatives");

            migrationBuilder.DropTable(
                name: "reminders");

            migrationBuilder.DropColumn(
                name: "cnpj",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "email",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "telefone",
                table: "companies");
        }
    }
}
