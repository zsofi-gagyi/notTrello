using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoWithDatabase.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssigneeProjects_AspNetUsers_AssigneeId1",
                table: "AssigneeProjects");

            migrationBuilder.DropIndex(
                name: "IX_AssigneeProjects_AssigneeId1",
                table: "AssigneeProjects");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f9aa147-f5ef-4a2d-9c5d-e02d57b606e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bb2aeee-ebff-44b4-8e7d-c9861fc1868e");

            migrationBuilder.DropColumn(
                name: "AssigneeId1",
                table: "AssigneeProjects");

            migrationBuilder.AlterColumn<string>(
                name: "AssigneeId",
                table: "AssigneeProjects",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a219375e-f555-4af1-a2bd-6b43a44ec00e", "972a3f1e-e9b2-4422-a73f-e17196b9db44", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b25dff8b-17b1-4112-b009-4751db6d3637", "f9d649cd-2c64-453a-87af-32cd3dde8356", "TodoUser", "TODOUSER" });

            migrationBuilder.AddForeignKey(
                name: "FK_AssigneeProjects_AspNetUsers_AssigneeId",
                table: "AssigneeProjects",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssigneeProjects_AspNetUsers_AssigneeId",
                table: "AssigneeProjects");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a219375e-f555-4af1-a2bd-6b43a44ec00e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b25dff8b-17b1-4112-b009-4751db6d3637");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssigneeId",
                table: "AssigneeProjects",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "AssigneeId1",
                table: "AssigneeProjects",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5bb2aeee-ebff-44b4-8e7d-c9861fc1868e", "f60fd50b-7b08-468f-b445-2805af0242cd", "TodoAdmin", "TODOADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4f9aa147-f5ef-4a2d-9c5d-e02d57b606e5", "d18eba2b-280f-4a07-be2e-9abf74efa17a", "TodoUser", "TODOUSER" });

            migrationBuilder.CreateIndex(
                name: "IX_AssigneeProjects_AssigneeId1",
                table: "AssigneeProjects",
                column: "AssigneeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AssigneeProjects_AspNetUsers_AssigneeId1",
                table: "AssigneeProjects",
                column: "AssigneeId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
