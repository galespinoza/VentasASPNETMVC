using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBoxes",
                columns: table => new
                {
                    IdBox = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Box = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBoxes", x => x.IdBox);
                });

            migrationBuilder.CreateTable(
                name: "TRecords_boxes",
                columns: table => new
                {
                    RegisterBoxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdBox = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRecords_boxes", x => x.RegisterBoxId);
                });

            migrationBuilder.CreateTable(
                name: "TReport_boxes",
                columns: table => new
                {
                    IdBoxReport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBox = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ticket = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Entry = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TReport_boxes", x => x.IdBoxReport);
                });

            migrationBuilder.CreateTable(
                name: "TIncome_boxes",
                columns: table => new
                {
                    IncomeBoxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    Ticket = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Entry = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TBoxesIdBox = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIncome_boxes", x => x.IncomeBoxId);
                    table.ForeignKey(
                        name: "FK_TIncome_boxes_TBoxes_TBoxesIdBox",
                        column: x => x.TBoxesIdBox,
                        principalTable: "TBoxes",
                        principalColumn: "IdBox",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TIncome_boxes_TBoxesIdBox",
                table: "TIncome_boxes",
                column: "TBoxesIdBox");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TIncome_boxes");

            migrationBuilder.DropTable(
                name: "TRecords_boxes");

            migrationBuilder.DropTable(
                name: "TReport_boxes");

            migrationBuilder.DropTable(
                name: "TBoxes");
        }
    }
}
