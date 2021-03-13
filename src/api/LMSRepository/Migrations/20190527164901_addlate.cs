using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSRepository.Migrations
{
    public partial class addlate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "62ef0d32-e662-4a76-bd02-5cfe43ea5c91");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f96c54ee-70bd-4339-aba3-73615b9f6e15");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e499849a-7b35-4cff-9e16-743233ed45a9");

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 9, "", "Late" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9f011117-ab11-4f0b-add0-fc073b41a159");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "154a37b1-371f-47f4-93c5-1888390f4a40");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "d12a305f-3d25-4ea2-94eb-72e15648175f");
        }
    }
}