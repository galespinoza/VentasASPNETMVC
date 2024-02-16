using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebt",
                table: "TPayments_clients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Monthly",
                table: "TPayments_clients",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDebt",
                table: "TPayments_clients",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TCustomer_interests",
                columns: table => new
                {
                    IdInterests = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Deadline = table.Column<DateTime>(nullable: false),
                    Debt = table.Column<decimal>(nullable: false),
                    Monthly = table.Column<decimal>(nullable: false),
                    Interests = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Canceled = table.Column<bool>(nullable: false),
                    IdCustomer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCustomer_interests", x => x.IdInterests);
                });

            migrationBuilder.CreateTable(
                name: "TCustomer_interests_reports",
                columns: table => new
                {
                    IdinterestReports = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Interests = table.Column<decimal>(nullable: false),
                    Payment = table.Column<decimal>(nullable: false),
                    Change = table.Column<decimal>(nullable: false),
                    Fee = table.Column<int>(nullable: false),
                    InterestDate = table.Column<DateTime>(nullable: false),
                    TicketInterest = table.Column<string>(nullable: true),
                    IdClient = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCustomer_interests_reports", x => x.IdinterestReports);
                });

            migrationBuilder.CreateTable(
                name: "TPayments_Reports_Customer_Interest",
                columns: table => new
                {
                    IdPaymentsInterest = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Interests = table.Column<decimal>(nullable: false),
                    Payment = table.Column<decimal>(nullable: false),
                    Change = table.Column<decimal>(nullable: false),
                    Fee = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Ticket = table.Column<string>(nullable: true),
                    IdUser = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    IdCustomer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TPayments_Reports_Customer_Interest", x => x.IdPaymentsInterest);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TCustomer_interests");

            migrationBuilder.DropTable(
                name: "TCustomer_interests_reports");

            migrationBuilder.DropTable(
                name: "TPayments_Reports_Customer_Interest");

            migrationBuilder.DropColumn(
                name: "DateDebt",
                table: "TPayments_clients");

            migrationBuilder.DropColumn(
                name: "Monthly",
                table: "TPayments_clients");

            migrationBuilder.DropColumn(
                name: "PreviousDebt",
                table: "TPayments_clients");
        }
    }
}
