using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoWithDatabase.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5df47f0-ff92-4b96-92d7-c814cfdde5a7", "f56f4796-4ea2-46f1-a2d7-f3649857ff2e", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db557772-ddca-4907-90cc-f75a9a9b2265", "5dbeb09b-5a3d-4eeb-a124-bf488758c8c0", "TodoUser", "TODOUSER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db557772-ddca-4907-90cc-f75a9a9b2265");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5df47f0-ff92-4b96-92d7-c814cfdde5a7");
        }
    }
}
