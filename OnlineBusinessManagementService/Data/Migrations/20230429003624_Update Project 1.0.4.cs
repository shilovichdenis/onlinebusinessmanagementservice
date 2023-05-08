using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProject104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_TimeSchedule_TimeScheduleId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "TimeSchedule");

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_WorkerId",
                table: "Schedules",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Schedules_TimeScheduleId",
                table: "Records",
                column: "TimeScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Schedules_TimeScheduleId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.CreateTable(
                name: "TimeSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeSchedule_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeSchedule_WorkerId",
                table: "TimeSchedule",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_TimeSchedule_TimeScheduleId",
                table: "Records",
                column: "TimeScheduleId",
                principalTable: "TimeSchedule",
                principalColumn: "Id");
        }
    }
}
