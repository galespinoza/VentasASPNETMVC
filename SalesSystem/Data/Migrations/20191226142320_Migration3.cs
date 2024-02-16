using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TClients",
                columns: table => new
                {
                    IdClient = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nid = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Direction = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Credit = table.Column<bool>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TClients", x => x.IdClient);
                });

            migrationBuilder.CreateTable(
                name: "TReports_clients",
                columns: table => new
                {
                    IdReport = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Debt = table.Column<decimal>(nullable: false),
                    Monthly = table.Column<decimal>(nullable: false),
                    Change = table.Column<decimal>(nullable: false),
                    LastPayment = table.Column<decimal>(nullable: false),
                    DatePayment = table.Column<DateTime>(nullable: false),
                    CurrentDebt = table.Column<decimal>(nullable: false),
                    DateDebt = table.Column<DateTime>(nullable: false),
                    Ticket = table.Column<string>(nullable: true),
                    Deadline = table.Column<DateTime>(nullable: false),
                    IdClient = table.Column<int>(nullable: false),
                    TClientsIdClient = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TReports_clients", x => x.IdReport);
                    table.ForeignKey(
                        name: "FK_TReports_clients_TClients_TClientsIdClient",
                        column: x => x.TClientsIdClient,
                        principalTable: "TClients",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TReports_clients_TClientsIdClient",
                table: "TReports_clients",
                column: "TClientsIdClient");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TReports_clients");

            migrationBuilder.DropTable(
                name: "TClients");
        }
    }
}
