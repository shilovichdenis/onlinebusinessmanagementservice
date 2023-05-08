using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineBusinessManagementService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectv103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementServices_Services_ServiceId",
                table: "AdvertisementServices");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Services",
                newName: "CategoryId");

            migrationBuilder.AddColumn<bool>(
                name: "inBlacklist",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Blacklist",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "AdvertisementServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Advertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: true),
                    TimeScheduleId = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    isConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    isSuccessful = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Records_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Records_TimeSchedule_TimeScheduleId",
                        column: x => x.TimeScheduleId,
                        principalTable: "TimeSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Records_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecordServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordServices_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecordServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "71882a8e-3d94-4dd9-97c4-bac815be9fc2", null, "Admin", "ADMIN" },
                    { "9d3bf9af-7384-4d35-b1f6-ab0552b327c0", null, "Manager", "MANAGER" },
                    { "e8a23752-e32d-446e-a3b5-7f4b49004dee", null, "Worker", "WORKER" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Парикмахерские услуги" },
                    { 2, "Ресницы" },
                    { 3, "Фитнес" },
                    { 4, "Барбершоп" },
                    { 5, "Брови" },
                    { 6, "Визаж" },
                    { 7, "Косметология, уход" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryId",
                table: "Services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Blacklist_BusinessId",
                table: "Blacklist",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Blacklist_ClientId",
                table: "Blacklist",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_BusinessId",
                table: "Advertisements",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_TimeScheduleId",
                table: "Records",
                column: "TimeScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserId",
                table: "Records",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_WorkerId",
                table: "Records",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordServices_RecordId",
                table: "RecordServices",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RecordServices_ServiceId",
                table: "RecordServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSchedule_WorkerId",
                table: "TimeSchedule",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_Businesses_BusinessId",
                table: "Advertisements",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementServices_Services_ServiceId",
                table: "AdvertisementServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Blacklist_Businesses_BusinessId",
                table: "Blacklist",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Blacklist_Clients_ClientId",
                table: "Blacklist",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_Businesses_BusinessId",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisementServices_Services_ServiceId",
                table: "AdvertisementServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Blacklist_Businesses_BusinessId",
                table: "Blacklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Blacklist_Clients_ClientId",
                table: "Blacklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Categories_CategoryId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "RecordServices");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "TimeSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Services_CategoryId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Blacklist_BusinessId",
                table: "Blacklist");

            migrationBuilder.DropIndex(
                name: "IX_Blacklist_ClientId",
                table: "Blacklist");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_BusinessId",
                table: "Advertisements");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71882a8e-3d94-4dd9-97c4-bac815be9fc2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d3bf9af-7384-4d35-b1f6-ab0552b327c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8a23752-e32d-446e-a3b5-7f4b49004dee");

            migrationBuilder.DropColumn(
                name: "inBlacklist",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Advertisements");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Services",
                newName: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Blacklist",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "AdvertisementServices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisementServices_Services_ServiceId",
                table: "AdvertisementServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
