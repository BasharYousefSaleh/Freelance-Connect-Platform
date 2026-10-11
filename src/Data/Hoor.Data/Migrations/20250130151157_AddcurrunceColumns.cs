using Microsoft.EntityFrameworkCore.Migrations;

namespace Hoor.Data.Migrations
{
    public partial class AddcurrunceColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentCurrncy",
                table: "Pyments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentCurrncyAmount",
                table: "Pyments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentImage",
                table: "Pyments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentCurrncy",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "PaymentCurrncyAmount",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "PaymentImage",
                table: "Pyments");
        }
    }
}
