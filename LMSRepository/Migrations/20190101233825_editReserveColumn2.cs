using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSLibrary.Migrations
{
    public partial class editReserveColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReserveAssets_LibraryCards_LibraryCardId",
                table: "ReserveAssets");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryCardId",
                table: "ReserveAssets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "53d54ca2-c1f9-4f5b-908b-e003e57b49b6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "93a489a4-2b82-4dce-adde-d0f1b3078b2e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "75bfeb66-efdd-4a2d-be3e-82a77dd5860c");

            migrationBuilder.AddForeignKey(
                name: "FK_ReserveAssets_LibraryCards_LibraryCardId",
                table: "ReserveAssets",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReserveAssets_LibraryCards_LibraryCardId",
                table: "ReserveAssets");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryCardId",
                table: "ReserveAssets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "db07c460-1faa-4502-8743-412bf278ad4c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "cef05786-c9dc-4cc7-a9ca-a64308d24baf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "5776602c-4080-4061-905e-3f014ac5bec8");

            migrationBuilder.AddForeignKey(
                name: "FK_ReserveAssets_LibraryCards_LibraryCardId",
                table: "ReserveAssets",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
