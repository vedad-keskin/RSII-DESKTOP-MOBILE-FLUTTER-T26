using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.Services.Migrations
{
    /// <inheritdoc />
    public partial class ordersexpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentCardId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentCardId",
                table: "Orders",
                column: "PaymentCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentCardIB180079_PaymentCardId",
                table: "Orders",
                column: "PaymentCardId",
                principalTable: "PaymentCardIB180079",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentCardIB180079_PaymentCardId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentCardId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentCardId",
                table: "Orders");
        }
    }
}
