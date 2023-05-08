using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectv106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "IsCompany",
                table: "Businesses");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Businesses");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompany",
                table: "Businesses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserId",
                table: "Businesses",
                column: "UserId");
        }
    }
}
