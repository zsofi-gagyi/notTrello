using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoWithDatabase.Migrations
{
    public partial class four : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b993447-9a78-4522-bb67-8bae182c20e0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c8af8a7-691e-44f6-a2e8-e19f060d8b6e");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssigneeProjects",
                columns: table => new
                {
                    AssigneeId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    AssigneeId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigneeProjects", x => new { x.AssigneeId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_AssigneeProjects_AspNetUsers_AssigneeId1",
                        column: x => x.AssigneeId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssigneeProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5bb2aeee-ebff-44b4-8e7d-c9861fc1868e", "f60fd50b-7b08-468f-b445-2805af0242cd", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f9aa147-f5ef-4a2d-9c5d-e02d57b606e5", "d18eba2b-280f-4a07-be2e-9abf74efa17a", "TodoUser", "TODOUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ProjectId",
                table: "Cards",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeProjects_AssigneeId1",
                table: "AssigneeProjects",
                column: "AssigneeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeProjects_ProjectId",
                table: "AssigneeProjects",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Projects_ProjectId",
                table: "Cards",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Projects_ProjectId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "AssigneeProjects");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ProjectId",
                table: "Cards");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f9aa147-f5ef-4a2d-9c5d-e02d57b606e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bb2aeee-ebff-44b4-8e7d-c9861fc1868e");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Cards");

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssigneeId = table.Column<string>(nullable: true),
                    Done = table.Column<short>(type: "SMALLINT", nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Urgent = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Todos_AspNetUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Todos_AssigneeId",
                table: "Todos",
                column: "AssigneeId");
        }
    }
}
