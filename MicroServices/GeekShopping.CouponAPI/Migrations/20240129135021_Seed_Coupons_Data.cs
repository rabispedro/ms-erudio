using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeekShopping.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCouponsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbl_coupon",
                columns: new[] { "id", "code", "discount_amount" },
                values: new object[,]
                {
                    { 1L, "ERUDIO_2023_12", 10.50m },
                    { 2L, "ERUDIO_2024_01", 50.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_coupon",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "tbl_coupon",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
