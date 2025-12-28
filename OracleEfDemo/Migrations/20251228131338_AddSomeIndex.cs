using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OracleEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "EFUSER",
                table: "EMPLOYEES",
                keyColumn: "Id",
                keyValue: "5cd9ecaa-4346-4b27-a9f8-a9a54e63a8d6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UpdatedDate" },
                values: new object[] { "9569bbea-c4f5-4c31-87ed-9559dd437894", "AQAAAAIAAYagAAAAEJmW4Xn3Kl2CcrQ6OILyvCygiPli4F4XKXuyWiiB007jrrD+4Cr9mObwBdxTF+yhow==", new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCTS_ProductName",
                schema: "EFUSER",
                table: "PRODUCTS",
                column: "ProductName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORIES_CategoryName",
                schema: "EFUSER",
                table: "CATEGORIES",
                column: "CategoryName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PRODUCTS_ProductName",
                schema: "EFUSER",
                table: "PRODUCTS");

            migrationBuilder.DropIndex(
                name: "IX_CATEGORIES_CategoryName",
                schema: "EFUSER",
                table: "CATEGORIES");

            migrationBuilder.UpdateData(
                schema: "EFUSER",
                table: "EMPLOYEES",
                keyColumn: "Id",
                keyValue: "5cd9ecaa-4346-4b27-a9f8-a9a54e63a8d6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "UpdatedDate" },
                values: new object[] { "b2fa1fce-8e35-43c5-afa0-6be534449cc4", "AQAAAAIAAYagAAAAEJreNPBACrbmnJNX324WtQ2Texherum6oxyExLBjQdJraWsfG8gxsaKUwN8QWjPC9w==", new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
