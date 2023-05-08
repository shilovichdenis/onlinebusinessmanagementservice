using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectv107 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagesPath",
                table: "Services",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Services",
                newName: "ImagesPath");
        }
    }
}
