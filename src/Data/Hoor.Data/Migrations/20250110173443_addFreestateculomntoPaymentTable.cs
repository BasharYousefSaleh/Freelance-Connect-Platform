using Microsoft.EntityFrameworkCore.Migrations;

namespace Hoor.Data.Migrations
{
    public partial class addFreestateculomntoPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FreeStatus",
                table: "Pyments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeStatus",
                table: "Pyments");
        }
    }
}
