using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class EnsureUniqueCodeAndSeedAutomaticDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAutomatic",
                table: "Discounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Amount", "Code", "IsAutomatic", "Multiplier" },
                values: new object[,]
                {
                    { new Guid("019c0efb-9347-75fa-8bf0-7211eebad895"), null, "AUTO_10_PERCENT", true, 0.1m },
                    { new Guid("019c0efb-9967-7c5b-9342-bdfb64a9e3e7"), null, "AUTO_20_PERCENT", true, 0.2m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Code",
                table: "Discounts",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Discounts_Code",
                table: "Discounts");

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("019c0efb-9347-75fa-8bf0-7211eebad895"));

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: new Guid("019c0efb-9967-7c5b-9342-bdfb64a9e3e7"));

            migrationBuilder.DropColumn(
                name: "IsAutomatic",
                table: "Discounts");
        }
    }
}
