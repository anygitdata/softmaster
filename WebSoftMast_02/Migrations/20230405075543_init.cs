using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebSoftMast_02.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NatSheets",
                columns: table => new
                {
                    NatSheetId = table.Column<long>(type: "bigint", nullable: false),
                    TrainNumber = table.Column<string>(type: "char(5)", nullable: false),
                    TrainIndexCombined = table.Column<string>(type: "varchar(100)", nullable: false),
                    FromStationName = table.Column<string>(type: "varchar(250)", nullable: true),
                    ToStationName = table.Column<string>(type: "varchar(250)", nullable: true),
                    LastStationName = table.Column<string>(type: "varchar(250)", nullable: true),
                    WhenLastOperation = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastOperationName = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NatSheets", x => x.NatSheetId);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false),
                    InvoiceNum = table.Column<string>(type: "varchar(100)", nullable: true),
                    PositionInTrain = table.Column<int>(type: "int", nullable: false),
                    CarNumber = table.Column<int>(type: "int", nullable: false),
                    FreightEtsngName = table.Column<string>(type: "varchar(250)", nullable: true),
                    FreightTotalWeightKg = table.Column<int>(type: "int", nullable: false),
                    NatSheetId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_Details_NatSheets_NatSheetId",
                        column: x => x.NatSheetId,
                        principalTable: "NatSheets",
                        principalColumn: "NatSheetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IND_PositionTrain",
                table: "Details",
                column: "PositionInTrain");

            migrationBuilder.CreateIndex(
                name: "IX_Details_NatSheetId",
                table: "Details",
                column: "NatSheetId");

            migrationBuilder.CreateIndex(
                name: "IND_TrainNumber",
                table: "NatSheets",
                column: "TrainNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "NatSheets");
        }
    }
}
