using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriticalConditionBackend.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfWorkingDays = table.Column<int>(type: "int", nullable: false),
                    NumberOfFailures = table.Column<int>(type: "int", nullable: false),
                    DownTime = table.Column<int>(type: "int", nullable: false),
                    Safety = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceCost = table.Column<double>(type: "float", nullable: false),
                    OperationCost = table.Column<double>(type: "float", nullable: false),
                    PurchasingCost = table.Column<double>(type: "float", nullable: false),
                    Detection = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FMEARiskScore = table.Column<int>(type: "int", nullable: false),
                    IsExcludedFromDSP = table.Column<bool>(type: "bit", nullable: false),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    IsIoT = table.Column<bool>(type: "bit", nullable: false),
                    SecurityRiskScore = table.Column<int>(type: "int", nullable: true),
                    AddedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEditBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuperUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EditsLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubUserUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubUserCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuperUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditsLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubUsers",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuperUserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubUsers", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "SuperUsers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalSpeciality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSubscriber = table.Column<bool>(type: "bit", nullable: false),
                    IsDataShareProgramMember = table.Column<bool>(type: "bit", nullable: false),
                    ExcludedDevicesNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperUsers", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "EditsLogs");

            migrationBuilder.DropTable(
                name: "SubUsers");

            migrationBuilder.DropTable(
                name: "SuperUsers");
        }
    }
}
