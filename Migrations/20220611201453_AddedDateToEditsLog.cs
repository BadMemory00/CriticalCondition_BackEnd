using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriticalConditionBackend.Migrations
{
    public partial class AddedDateToEditsLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActionDate",
                table: "EditsLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionDate",
                table: "EditsLogs");
        }
    }
}
