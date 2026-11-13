using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eCommerce.Services.Migrations
{
    /// <inheritdoc />
    public partial class Coupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxUses = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CouponUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouponId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CouponUsages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "DiscountPercentage", "IsActive", "MaxUses", "UpdatedAt", "ValidFrom", "ValidTo" },
                values: new object[,]
                {
                    { 1, "SAVE", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Ten percent discount", 10m, true, 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2030, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc) },
                    { 2, "WELCOME", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Twenty-five percent discount", 25m, true, 5, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2030, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc) },
                    { 3, "EXPIRED", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Expired coupon", 20m, true, 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc) },
                    { 4, "FUTURE", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Coupon that is not valid yet", 15m, true, 10, null, new DateTime(2035, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2035, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc) },
                    { 5, "INACTIVE", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Inactive coupon", 30m, false, 10, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2030, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc) },
                    { 6, "LIMIT", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Coupon with its only use already recorded", 50m, true, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2030, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "CouponUsages",
                columns: new[] { "Id", "CouponId", "DiscountAmount", "UsedAt", "UserId" },
                values: new object[] { 1, 6, 10m, new DateTime(2026, 9, 2, 12, 0, 0, 0, DateTimeKind.Utc), 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_Code",
                table: "Coupons",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_CouponId_UserId",
                table: "CouponUsages",
                columns: new[] { "CouponId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponUsages_UserId",
                table: "CouponUsages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponUsages");

            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
