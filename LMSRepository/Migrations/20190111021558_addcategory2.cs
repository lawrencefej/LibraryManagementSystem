using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSLibrary.Migrations
{
    public partial class addcategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "ReserveAssets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "LibraryAssets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Checkouts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LibraryAssetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7ee05221-6ad2-45c1-ab57-115877897683");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "9c221167-ec98-49ca-a411-fc64638e2adc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "97ce744a-81ff-4b7d-a747-071216fe5bb0");

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 3, "The last available copy has been checked out", "Checkedout" },
                    { 4, "The last available copy has been checked out", "Returned" },
                    { 5, "The last available copy has been checked out", "Expired" },
                    { 6, "The last available copy has been checked out", "Canceled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReserveAssets_StatusId",
                table: "ReserveAssets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryAssets_CategoryId",
                table: "LibraryAssets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_StatusId",
                table: "Checkouts",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_Statuses_StatusId",
                table: "Checkouts",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryAssets_Category_CategoryId",
                table: "LibraryAssets",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReserveAssets_Statuses_StatusId",
                table: "ReserveAssets",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_Statuses_StatusId",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_LibraryAssets_Category_CategoryId",
                table: "LibraryAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_ReserveAssets_Statuses_StatusId",
                table: "ReserveAssets");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_ReserveAssets_StatusId",
                table: "ReserveAssets");

            migrationBuilder.DropIndex(
                name: "IX_LibraryAssets_CategoryId",
                table: "LibraryAssets");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_StatusId",
                table: "Checkouts");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ReserveAssets");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "LibraryAssets");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Checkouts");

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
        }
    }
}
