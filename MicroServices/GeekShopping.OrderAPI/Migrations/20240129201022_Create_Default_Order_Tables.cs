using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateDefaultOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_order_header",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userid = table.Column<string>(name: "user_id", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    couponcode = table.Column<string>(name: "coupon_code", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    purchaseamount = table.Column<decimal>(name: "purchase_amount", type: "decimal(65,30)", nullable: false),
                    discountamount = table.Column<decimal>(name: "discount_amount", type: "decimal(65,30)", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastname = table.Column<string>(name: "last_name", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    purchasedate = table.Column<DateTime>(name: "purchase_date", type: "datetime(6)", nullable: false),
                    ordertime = table.Column<DateTime>(name: "order_time", type: "datetime(6)", nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cardnumber = table.Column<string>(name: "card_number", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cvv = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    expirymonthyear = table.Column<string>(name: "expiry_month_year", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    totalitems = table.Column<int>(name: "total_items", type: "int", nullable: false),
                    paymentstatus = table.Column<bool>(name: "payment_status", type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_header", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tbl_order_detail",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    productid = table.Column<long>(name: "product_id", type: "bigint", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    productname = table.Column<string>(name: "product_name", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_order_detail_tbl_order_header_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "tbl_order_header",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_detail_OrderHeaderId",
                table: "tbl_order_detail",
                column: "OrderHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_order_detail");

            migrationBuilder.DropTable(
                name: "tbl_order_header");
        }
    }
}
