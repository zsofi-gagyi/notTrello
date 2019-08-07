using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoWithDatabase.Migrations
{
    public partial class sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssigneeCards_AspNetUsers_AssigneeId1",
                table: "AssigneeCards");

            migrationBuilder.DropIndex(
                name: "IX_AssigneeCards_AssigneeId1",
                table: "AssigneeCards");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a219375e-f555-4af1-a2bd-6b43a44ec00e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b25dff8b-17b1-4112-b009-4751db6d3637");

            migrationBuilder.DropColumn(
                name: "AssigneeId1",
                table: "AssigneeCards");

            migrationBuilder.AlterColumn<string>(
                name: "AssigneeId",
                table: "AssigneeCards",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "887fc95a-84ce-4845-a86a-38bb67b14664", "c192eb09-fc12-48eb-b3a7-9ba7be19a9f3", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6b9b6eea-1143-4744-b6e0-681b0af03601", "279faf42-1156-460c-a67c-1a22e2baf1cc", "TodoUser", "TODOUSER" });

            migrationBuilder.AddForeignKey(
                name: "FK_AssigneeCards_AspNetUsers_AssigneeId",
                table: "AssigneeCards",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssigneeCards_AspNetUsers_AssigneeId",
                table: "AssigneeCards");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b9b6eea-1143-4744-b6e0-681b0af03601");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "887fc95a-84ce-4845-a86a-38bb67b14664");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssigneeId",
                table: "AssigneeCards",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "AssigneeId1",
                table: "AssigneeCards",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a219375e-f555-4af1-a2bd-6b43a44ec00e", "972a3f1e-e9b2-4422-a73f-e17196b9db44", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b25dff8b-17b1-4112-b009-4751db6d3637", "f9d649cd-2c64-453a-87af-32cd3dde8356", "TodoUser", "TODOUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeCards_AssigneeId1",
                table: "AssigneeCards",
                column: "AssigneeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AssigneeCards_AspNetUsers_AssigneeId1",
                table: "AssigneeCards",
                column: "AssigneeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
