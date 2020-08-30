using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace leave_management.Data.Migrations
{
    public partial class addedDefaultDaysAndperiod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultDays",
                table: "leaveTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "period",
                table: "leaveAllocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DetailsLeaveTypeVm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsLeaveTypeVm", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsLeaveTypeVm");

            migrationBuilder.DropColumn(
                name: "DefaultDays",
                table: "leaveTypes");

            migrationBuilder.DropColumn(
                name: "period",
                table: "leaveAllocations");
        }
    }
}
