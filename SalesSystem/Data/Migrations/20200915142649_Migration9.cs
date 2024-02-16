using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TTemporary_shopping",
                columns: table => new
                {
                    IdTemporary = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    IdProvider = table.Column<int>(nullable: false),
                    Provider = table.Column<string>(nullable: false),
                    IdUser = table.Column<string>(nullable: true),
                    Image = table.Column<byte[]>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TTemporary_shopping", x => x.IdTemporary);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TTemporary_shopping");
        }
    }
}
