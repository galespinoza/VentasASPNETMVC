using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TReports_clients_TClients_TClientsIdClient",
                table: "TReports_clients");

            migrationBuilder.DropColumn(
                name: "IdClient",
                table: "TReports_clients");

            migrationBuilder.AlterColumn<int>(
                name: "TClientsIdClient",
                table: "TReports_clients",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TPayments_clients",
                columns: table => new
                {
                    IdPayments = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Debt = table.Column<decimal>(nullable: false),
                    Change = table.Column<decimal>(nullable: false),
                    Payment = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CurrentDebt = table.Column<decimal>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: false),
                    Ticket = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    IdClient = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPayments_clients", x => x.IdPayments);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TReports_clients_TClients_TClientsIdClient",
                table: "TReports_clients",
                column: "TClientsIdClient",
                principalTable: "TClients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TReports_clients_TClients_TClientsIdClient",
                table: "TReports_clients");

            migrationBuilder.DropTable(
                name: "TPayments_clients");

            migrationBuilder.AlterColumn<int>(
                name: "TClientsIdClient",
                table: "TReports_clients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "IdClient",
                table: "TReports_clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TReports_clients_TClients_TClientsIdClient",
                table: "TReports_clients",
                column: "TClientsIdClient",
                principalTable: "TClients",
                principalColumn: "IdClient",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
