using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMSLibrary.Migrations
{
    public partial class addedReserveReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCheckedOut",
                table: "ReserveAssets",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedOut",
                table: "ReserveAssets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6e3a246e-750c-4d93-8d66-95ab99b45336");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2b10218f-6ea1-4c01-a2b8-37d7df225d1a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "97847e78-1b85-4ba7-8642-20caa105a82f");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCheckedOut",
                table: "ReserveAssets");

            migrationBuilder.DropColumn(
                name: "IsCheckedOut",
                table: "ReserveAssets");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "9304bc4c-0e39-4f42-8a89-474d1a9e9f0d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "bcd55dc6-15f5-4dbf-ad87-b9ef4c23a877");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "12f4b221-21a0-4442-92c0-3c0c8dc0f16b");
        }
    }
}
