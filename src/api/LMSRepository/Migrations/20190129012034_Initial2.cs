using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSRepository.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Statuses_StatusId",
                table: "Checkouts");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Checkouts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "38b6e7e6-9539-48d9-90df-a87030ff2667");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "00409502-23c6-4c25-a61b-a4e0342a6348");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "59e78112-f6b7-458d-977d-e30a0a4aa30f");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Statuses_StatusId",
                table: "Checkouts",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Statuses_StatusId",
                table: "Checkouts");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Checkouts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "05e68eb1-e105-4ed9-9beb-9f2039807c1f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "09f78f3b-594e-4609-8e20-93c3740ecac5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "da08a154-9db0-4313-9dce-8c83b9aafe32");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Statuses_StatusId",
                table: "Checkouts",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}