using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSLibrary.Migrations
{
    public partial class fixCheckout2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryAssets_LibraryAssetId1",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryCards_LibraryCardId1",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_LibraryAssetId1",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_LibraryCardId1",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "LibraryAssetId1",
                table: "Checkouts");

            migrationBuilder.DropColumn(
                name: "LibraryCardId1",
                table: "Checkouts");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryCardId",
                table: "Checkouts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LibraryAssetId",
                table: "Checkouts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "73da8458-0cee-4642-b9ea-18040cb46eca");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7803eaba-48fb-47c4-9859-4faaaccf9134");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4cd1e512-f7ac-41e3-81c2-6881cbdec331");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_LibraryAssetId",
                table: "Checkouts",
                column: "LibraryAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_LibraryCardId",
                table: "Checkouts",
                column: "LibraryCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryAssets_LibraryAssetId",
                table: "Checkouts",
                column: "LibraryAssetId",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryCards_LibraryCardId",
                table: "Checkouts",
                column: "LibraryCardId",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryAssets_LibraryAssetId",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryCards_LibraryCardId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_LibraryAssetId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_LibraryCardId",
                table: "Checkouts");

            migrationBuilder.AlterColumn<string>(
                name: "LibraryCardId",
                table: "Checkouts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "LibraryAssetId",
                table: "Checkouts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "LibraryAssetId1",
                table: "Checkouts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LibraryCardId1",
                table: "Checkouts",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "24662dee-32a5-4b97-9e29-6cc845b7a117");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b28221ee-b5ae-4558-87cb-cea26926c9d5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "28097557-82f3-44f3-b8d0-bccc96e983e2");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_LibraryAssetId1",
                table: "Checkouts",
                column: "LibraryAssetId1");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_LibraryCardId1",
                table: "Checkouts",
                column: "LibraryCardId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryAssets_LibraryAssetId1",
                table: "Checkouts",
                column: "LibraryAssetId1",
                principalTable: "LibraryAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryCards_LibraryCardId1",
                table: "Checkouts",
                column: "LibraryCardId1",
                principalTable: "LibraryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
