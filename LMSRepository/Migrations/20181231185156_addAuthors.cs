using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSLibrary.Migrations
{
    public partial class addAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryAssetTypes");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "LibraryAssets");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "LibraryAssets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ca75a134-d741-45b1-b8b0-b23de86cf324");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "fe07bee4-28f7-48ca-94b7-79f81d938d0f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "b28979da-806c-49cd-a288-717293cc375b");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssets_AuthorId",
                table: "LibraryAssets",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_Authors_AuthorId",
                table: "LibraryAssets",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_Authors_AuthorId",
                table: "LibraryAssets");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_LibraryAssets_AuthorId",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "LibraryAssets");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "LibraryAssets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LibraryAssets",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "LibraryAssets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LibraryAssetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssetTypeId = table.Column<int>(nullable: false),
                    LibraryAssetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryAssetTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryAssetTypes_AssetTypes_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "AssetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryAssetTypes_LibraryAssets_LibraryAssetId",
                        column: x => x.LibraryAssetId,
                        principalTable: "LibraryAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5511e0c6-9a8e-47c2-8383-8b8b459ade34");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "075de63a-c096-43ff-871d-f9dc79e2dc13");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "0da9a458-5233-420d-bbba-a8fe1871370f");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssetTypes_AssetTypeId",
                table: "LibraryAssetTypes",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssetTypes_LibraryAssetId",
                table: "LibraryAssetTypes",
                column: "LibraryAssetId");
        }
    }
}
