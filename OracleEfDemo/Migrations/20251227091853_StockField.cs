using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OracleEfDemo.Migrations
{
    /// <inheritdoc />
    public partial class StockField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "EFUSER",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "088245e8-bfb9-4991-82d6-054b24478260");

            migrationBuilder.DeleteData(
                schema: "EFUSER",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49b521b0-bb2b-40cf-94e7-21fbb509d7a8");

            migrationBuilder.DeleteData(
                schema: "EFUSER",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62774381-acc8-4c74-8476-63a6bb780122");

            migrationBuilder.DeleteData(
                schema: "EFUSER",
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b426aa28-4802-48fb-9dd7-133225e6d068");

            migrationBuilder.RenameColumn(
                name: "StockQuantity",
                schema: "EFUSER",
                table: "STOCK_LOG",
                newName: "StockAfter");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                schema: "EFUSER",
                table: "STOCK_LOG",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuantityChange",
                schema: "EFUSER",
                table: "STOCK_LOG",
                type: "DECIMAL(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                schema: "EFUSER",
                table: "EMPLOYEES",
                keyColumn: "Id",
                keyValue: "5cd9ecaa-4346-4b27-a9f8-a9a54e63a8d6",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "b2fa1fce-8e35-43c5-afa0-6be534449cc4", new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEJreNPBACrbmnJNX324WtQ2Texherum6oxyExLBjQdJraWsfG8gxsaKUwN8QWjPC9w==", new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "EFUSER",
                table: "STOCK_LOG");

            migrationBuilder.DropColumn(
                name: "QuantityChange",
                schema: "EFUSER",
                table: "STOCK_LOG");

            migrationBuilder.RenameColumn(
                name: "StockAfter",
                schema: "EFUSER",
                table: "STOCK_LOG",
                newName: "StockQuantity");

            migrationBuilder.InsertData(
                schema: "EFUSER",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "088245e8-bfb9-4991-82d6-054b24478260", null, "CustomerAuthorization", "ROLE4" },
                    { "49b521b0-bb2b-40cf-94e7-21fbb509d7a8", null, "OrderAuthorization", "ORDER" },
                    { "62774381-acc8-4c74-8476-63a6bb780122", null, "SalaryAuthorization", "ROLE3" },
                    { "b426aa28-4802-48fb-9dd7-133225e6d068", null, "ProductAuthorization", "ROLE2" }
                });

            migrationBuilder.UpdateData(
                schema: "EFUSER",
                table: "EMPLOYEES",
                keyColumn: "Id",
                keyValue: "5cd9ecaa-4346-4b27-a9f8-a9a54e63a8d6",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash", "UpdatedDate" },
                values: new object[] { "c2ca128e-9b03-4494-9f08-d857c4eb1b84", new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELs8qLAdK46pvmLphiBPKd1eJnNs2PsRuZJfJ0GXRm1horcV+sn9DfhImPUMGo7Klg==", new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
