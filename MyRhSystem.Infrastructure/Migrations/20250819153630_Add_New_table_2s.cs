using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyRhSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_New_table_2s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "job_levels",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "job_levels",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "job_levels",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "job_levels",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "job_levels",
                keyColumn: "id",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "job_levels",
                columns: new[] { "id", "created_at", "nome", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(5508), "Estagiário", 1, null },
                    { 2, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6197), "Júnior", 2, null },
                    { 3, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6271), "Pleno", 3, null },
                    { 4, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6272), "Sênior", 4, null },
                    { 5, new DateTime(2025, 8, 19, 15, 28, 22, 570, DateTimeKind.Utc).AddTicks(6272), "Trainer", 5, null }
                });
        }
    }
}
