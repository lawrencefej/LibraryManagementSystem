using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSLibrary.Migrations
{
    public partial class addreservedstatus2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReserveAssets_LibraryAssets_LibraryAssetId",
                table: "ReserveAssets");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryAssetId",
                table: "ReserveAssets",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "545f689e-0831-4fe3-a760-f726a9c74c66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b4ec696e-18bc-4069-8c5f-5dabca638541");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9273baa6-5dd7-4cba-9d92-cf2d438426f4");

            migrationBuilder.AddForeignKey(
                name: "FK_ReserveAssets_LibraryAssets_LibraryAssetId",
                table: "ReserveAssets",
                column: "LibraryAssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReserveAssets_LibraryAssets_LibraryAssetId",
                table: "ReserveAssets");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryAssetId",
                table: "ReserveAssets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c3ecc7c0-125b-494d-8e15-6ba6c30bf6cb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f4bfd943-e3bf-43b2-9284-6f1996f584f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "ea2e86ba-4372-4fba-834d-e0d7f3e2387b");

            migrationBuilder.AddForeignKey(
                name: "FK_ReserveAssets_LibraryAssets_LibraryAssetId",
                table: "ReserveAssets",
                column: "LibraryAssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
