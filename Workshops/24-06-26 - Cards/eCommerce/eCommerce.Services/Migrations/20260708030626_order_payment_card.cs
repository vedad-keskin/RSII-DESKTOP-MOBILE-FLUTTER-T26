using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.Services.Migrations
{
    /// <inheritdoc />
    public partial class order_payment_card : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentCardIB180079Id",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentCardIB180079Id",
                table: "Orders",
                column: "PaymentCardIB180079Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentCardIB180079_PaymentCardIB180079Id",
                table: "Orders",
                column: "PaymentCardIB180079Id",
                principalTable: "PaymentCardIB180079",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentCardIB180079_PaymentCardIB180079Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentCardIB180079Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentCardIB180079Id",
                table: "Orders");
        }
    }
}
