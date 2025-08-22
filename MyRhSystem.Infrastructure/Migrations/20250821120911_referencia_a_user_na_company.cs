using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class referencia_a_user_na_company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "created_by_user_id",
                table: "companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_created_by_user_id",
                table: "companies",
                column: "created_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_companies_users_created_by_user_id",
                table: "companies",
                column: "created_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_companies_users_created_by_user_id",
                table: "companies");

            migrationBuilder.DropIndex(
                name: "IX_companies_created_by_user_id",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "created_by_user_id",
                table: "companies");
        }
    }
}
