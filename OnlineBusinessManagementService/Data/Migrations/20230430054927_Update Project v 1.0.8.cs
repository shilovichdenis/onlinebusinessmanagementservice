using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectv108 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeForSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeInt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeForSchedule", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TimeForSchedule",
                columns: new[] { "Id", "TimeInt", "TimeString" },
                values: new object[,]
                {
                    { 1, 0, "00:00" },
                    { 2, 1, "01:00" },
                    { 3, 2, "02:00" },
                    { 4, 3, "03:00" },
                    { 5, 4, "04:00" },
                    { 6, 5, "05:00" },
                    { 7, 6, "06:00" },
                    { 8, 7, "07:00" },
                    { 9, 8, "08:00" },
                    { 10, 9, "09:00" },
                    { 11, 10, "10:00" },
                    { 12, 11, "11:00" },
                    { 13, 12, "12:00" },
                    { 14, 13, "13:00" },
                    { 15, 14, "14:00" },
                    { 16, 15, "15:00" },
                    { 17, 16, "16:00" },
                    { 18, 17, "17:00" },
                    { 19, 18, "18:00" },
                    { 20, 19, "19:00" },
                    { 21, 20, "20:00" },
                    { 22, 21, "21:00" },
                    { 23, 22, "22:00" },
                    { 24, 23, "23:00" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeForSchedule");
        }
    }
}
