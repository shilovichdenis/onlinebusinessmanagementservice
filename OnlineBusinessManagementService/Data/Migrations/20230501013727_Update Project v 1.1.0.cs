using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectv110 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordServices_Services_ServiceId",
                table: "RecordServices");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordServices_WorkerServices_ServiceId",
                table: "RecordServices",
                column: "ServiceId",
                principalTable: "WorkerServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecordServices_WorkerServices_ServiceId",
                table: "RecordServices");

            migrationBuilder.AddForeignKey(
                name: "FK_RecordServices_Services_ServiceId",
                table: "RecordServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
