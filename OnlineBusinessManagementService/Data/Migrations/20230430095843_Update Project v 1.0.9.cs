using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectv109 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagesPath",
                table: "WorkerReviews");

            migrationBuilder.DropColumn(
                name: "ImagesPath",
                table: "BusinessReviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagesPath",
                table: "WorkerReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagesPath",
                table: "BusinessReviews",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
