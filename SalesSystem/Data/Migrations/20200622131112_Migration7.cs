using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "TReports_clients",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "InitialDate",
                table: "TCustomer_interests",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TProviders",
                columns: table => new
                {
                    IdProvider = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Direction = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TProviders", x => x.IdProvider);
                });

            migrationBuilder.CreateTable(
                name: "TReports_provider",
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
                    Deadline = table.Column<DateTime>(nullable: true),
                    TProvidersIdProvider = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TReports_provider", x => x.IdReport);
                    table.ForeignKey(
                        name: "FK_TReports_provider_TProviders_TProvidersIdProvider",
                        column: x => x.TProvidersIdProvider,
                        principalTable: "TProviders",
                        principalColumn: "IdProvider",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TReports_provider_TProvidersIdProvider",
                table: "TReports_provider",
                column: "TProvidersIdProvider");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TReports_provider");

            migrationBuilder.DropTable(
                name: "TProviders");

            migrationBuilder.DropColumn(
                name: "InitialDate",
                table: "TCustomer_interests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Deadline",
                table: "TReports_clients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
