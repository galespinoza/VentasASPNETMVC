using Microsoft.EntityFrameworkCore.Migrations;

namespace SalesSystem.Data.Migrations
{
    public partial class Migration16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TIncome_boxes_TBoxes_TBoxesIdBox",
                table: "TIncome_boxes");

            migrationBuilder.AlterColumn<int>(
                name: "TBoxesIdBox",
                table: "TIncome_boxes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdUser",
                table: "TIncome_boxes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TIncome_boxes_TBoxes_TBoxesIdBox",
                table: "TIncome_boxes",
                column: "TBoxesIdBox",
                principalTable: "TBoxes",
                principalColumn: "IdBox",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TIncome_boxes_TBoxes_TBoxesIdBox",
                table: "TIncome_boxes");

            migrationBuilder.AlterColumn<int>(
                name: "TBoxesIdBox",
                table: "TIncome_boxes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdUser",
                table: "TIncome_boxes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TIncome_boxes_TBoxes_TBoxesIdBox",
                table: "TIncome_boxes",
                column: "TBoxesIdBox",
                principalTable: "TBoxes",
                principalColumn: "IdBox",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
