using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_referencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_legal_representatives_companies_company_id",
                table: "legal_representatives");

            migrationBuilder.DropIndex(
                name: "IX_legal_representatives_company_id",
                table: "legal_representatives");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "legal_representatives");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "legal_representatives",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "legal_representatives",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "representative_id",
                table: "companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_legal_representatives_CPF",
                table: "legal_representatives",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_representative_id",
                table: "companies",
                column: "representative_id",
                unique: true,
                filter: "[representative_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_companies_legal_representatives_representative_id",
                table: "companies",
                column: "representative_id",
                principalTable: "legal_representatives",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_companies_legal_representatives_representative_id",
                table: "companies");

            migrationBuilder.DropIndex(
                name: "IX_legal_representatives_CPF",
                table: "legal_representatives");

            migrationBuilder.DropIndex(
                name: "IX_companies_representative_id",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "representative_id",
                table: "companies");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "legal_representatives",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "legal_representatives",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "company_id",
                table: "legal_representatives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "companies",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_legal_representatives_company_id",
                table: "legal_representatives",
                column: "company_id");

            migrationBuilder.AddForeignKey(
                name: "FK_legal_representatives_companies_company_id",
                table: "legal_representatives",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
