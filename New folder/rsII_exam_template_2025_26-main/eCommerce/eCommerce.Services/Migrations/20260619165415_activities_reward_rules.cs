using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eCommerce.Services.Migrations
{
    /// <inheritdoc />
    public partial class activities_reward_rules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    RewardTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxDaysToComplete = table.Column<int>(type: "int", nullable: false),
                    NumberOfPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardRules_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RewardTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserActivities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Description", "DueDate", "Name" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Organizacija sastanka" },
                    { 2, "", new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Izrada prezentacije" },
                    { 3, "", new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Analiza projekta" }
                });

            migrationBuilder.InsertData(
                table: "RewardRules",
                columns: new[] { "Id", "ActivityId", "MaxDaysToComplete", "NumberOfPoints", "RewardTitle" },
                values: new object[,]
                {
                    { 1, 1, 5, 10, "Organizacija sastanka - 10 points" },
                    { 2, 2, 15, 20, "Izrada prezentacije - 20 points" },
                    { 3, 3, 25, 30, "Analiza projekta - 30 points" }
                });

            migrationBuilder.InsertData(
                table: "UserActivities",
                columns: new[] { "Id", "ActivityId", "CompletedAt", "DateAssigned", "Note", "RewardTitle", "RewardedAt", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Random note", "Organizacija sastanka - 10 points", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Completed", 1 },
                    { 2, 2, null, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Random note", null, null, "Cancelled", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RewardRules_ActivityId",
                table: "RewardRules",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_ActivityId",
                table: "UserActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId",
                table: "UserActivities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardRules");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
