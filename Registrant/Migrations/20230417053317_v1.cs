using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Registrant.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contragents",
                columns: table => new
                {
                    idContragent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Active = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    ServiceInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contragents", x => x.idContragent);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    idDriver = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Family = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Patronymic = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    idContragent = table.Column<int>(type: "int", nullable: true),
                    Attorney = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Auto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AutoNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Passport = table.Column<string>(type: "text", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true),
                    Active = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    ServiceInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.idDriver);
                });

            migrationBuilder.CreateTable(
                name: "Engine",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    ActualAppVer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActualBdVer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engine", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Time",
                columns: table => new
                {
                    idTime = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimePlanRegist = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTimeFactRegist = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTimeArrive = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTimeLoad = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTimeEndLoad = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateTimeLeft = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time", x => x.idTime);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LevelAccess = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.idUser);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    idShipment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idDriver = table.Column<int>(type: "int", nullable: true),
                    idContragent = table.Column<int>(type: "int", nullable: true),
                    idTime = table.Column<int>(type: "int", nullable: false),
                    NumRealese = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CountPodons = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PacketDocuments = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TochkaLoad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Nomenclature = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TypeLoad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StoreKeeper = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Active = table.Column<string>(type: "nchar(1)", fixedLength: true, maxLength: 1, nullable: true),
                    ServiceInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.idShipment);
                    table.ForeignKey(
                        name: "FK_Shipment_Contragents",
                        column: x => x.idContragent,
                        principalTable: "Contragents",
                        principalColumn: "idContragent",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_Drivers",
                        column: x => x.idDriver,
                        principalTable: "Drivers",
                        principalColumn: "idDriver",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_Time",
                        column: x => x.idTime,
                        principalTable: "Time",
                        principalColumn: "idTime",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_idContragent",
                table: "Shipment",
                column: "idContragent");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_idDriver",
                table: "Shipment",
                column: "idDriver");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_idTime",
                table: "Shipment",
                column: "idTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Engine");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Contragents");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Time");
        }
    }
}
