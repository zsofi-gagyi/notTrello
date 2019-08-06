using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoWithDatabase.Migrations
{
    public partial class three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db557772-ddca-4907-90cc-f75a9a9b2265");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5df47f0-ff92-4b96-92d7-c814cfdde5a7");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Done = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssigneeCards",
                columns: table => new
                {
                    AssigneeId = table.Column<Guid>(nullable: false),
                    CardId = table.Column<Guid>(nullable: false),
                    AssigneeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigneeCards", x => new { x.AssigneeId, x.CardId });
                    table.ForeignKey(
                        name: "FK_AssigneeCards_AspNetUsers_AssigneeId1",
                        column: x => x.AssigneeId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssigneeCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b993447-9a78-4522-bb67-8bae182c20e0", "aa73af33-6e85-445d-a67d-5e30da3cc604", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c8af8a7-691e-44f6-a2e8-e19f060d8b6e", "cc05092f-528a-4357-83d9-0e4752aeeedf", "TodoUser", "TODOUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeCards_AssigneeId1",
                table: "AssigneeCards",
                column: "AssigneeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeCards_CardId",
                table: "AssigneeCards",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssigneeCards");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b993447-9a78-4522-bb67-8bae182c20e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c8af8a7-691e-44f6-a2e8-e19f060d8b6e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e5df47f0-ff92-4b96-92d7-c814cfdde5a7", "f56f4796-4ea2-46f1-a2d7-f3649857ff2e", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db557772-ddca-4907-90cc-f75a9a9b2265", "5dbeb09b-5a3d-4eeb-a124-bf488758c8c0", "TodoUser", "TODOUSER" });
        }
    }
}
