using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployManagement.Migrations
{
    /// <inheritdoc />
    public partial class initialMigrationdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e3c42cc-0aef-402d-b25f-a60af60b9324");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20b51090-e650-406c-8984-3efad84d4256");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97ef65b5-33a8-4947-aa29-f1de4709b4c6");

            migrationBuilder.DropColumn(
                name: "DepartrmentId",
                table: "Mangers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2b03bb38-d3e0-4ef2-9bdf-59cda9e11181", "2", "User", "User" },
                    { "5342619e-c315-4398-a6d9-a5ca47facb0c", "3", "HR", "HR" },
                    { "bb93612b-197d-4eaf-b35f-a7b10678de93", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b03bb38-d3e0-4ef2-9bdf-59cda9e11181");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5342619e-c315-4398-a6d9-a5ca47facb0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb93612b-197d-4eaf-b35f-a7b10678de93");

            migrationBuilder.AddColumn<int>(
                name: "DepartrmentId",
                table: "Mangers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e3c42cc-0aef-402d-b25f-a60af60b9324", "3", "HR", "HR" },
                    { "20b51090-e650-406c-8984-3efad84d4256", "2", "User", "User" },
                    { "97ef65b5-33a8-4947-aa29-f1de4709b4c6", "1", "Admin", "Admin" }
                });
        }
    }
}
