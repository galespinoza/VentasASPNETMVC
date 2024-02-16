using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TShopping",
                columns: table => new
                {
                    IdShopping = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    IdProvider = table.Column<int>(nullable: false),
                    IdUser = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    Ticket = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TShopping", x => x.IdShopping);
                });

            migrationBuilder.CreateTable(
                name: "TTemporary_product",
                columns: table => new
                {
                    IdTemporary = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdShopping = table.Column<int>(nullable: false),
                    IdUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTemporary_product", x => x.IdTemporary);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TShopping");

            migrationBuilder.DropTable(
                name: "TTemporary_product");
        }
    }
}
