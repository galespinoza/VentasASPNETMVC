using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TReports_provider_TProviders_TProvidersIdProvider",
                table: "TReports_provider");

            migrationBuilder.AlterColumn<int>(
                name: "TProvidersIdProvider",
                table: "TReports_provider",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InitialDate",
                table: "TCustomer_interests",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "TPayments_provider",
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
                    DateDebt = table.Column<DateTime>(nullable: false),
                    Monthly = table.Column<decimal>(nullable: false),
                    PreviousDebt = table.Column<decimal>(nullable: false),
                    Ticket = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    IdProvider = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPayments_provider", x => x.IdPayments);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TReports_provider_TProviders_TProvidersIdProvider",
                table: "TReports_provider",
                column: "TProvidersIdProvider",
                principalTable: "TProviders",
                principalColumn: "IdProvider",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TReports_provider_TProviders_TProvidersIdProvider",
                table: "TReports_provider");

            migrationBuilder.DropTable(
                name: "TPayments_provider");

            migrationBuilder.AlterColumn<int>(
                name: "TProvidersIdProvider",
                table: "TReports_provider",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InitialDate",
                table: "TCustomer_interests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TReports_provider_TProviders_TProvidersIdProvider",
                table: "TReports_provider",
                column: "TProvidersIdProvider",
                principalTable: "TProviders",
                principalColumn: "IdProvider",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
