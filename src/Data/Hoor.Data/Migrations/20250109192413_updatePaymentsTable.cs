using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hoor.Data.Migrations
{
    public partial class updatePaymentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pyments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ContractID",
                table: "Pyments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountAfterPlatformFee",
                table: "Pyments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountAfterStripeFee",
                table: "Pyments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountFreelacer",
                table: "Pyments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Pyments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Pyments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pyments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Pyments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Pyments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PlatformBaqshes",
                table: "Pyments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pyments_IsDeleted",
                table: "Pyments",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pyments_IsDeleted",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "AmountAfterPlatformFee",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "AmountAfterStripeFee",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "AmountFreelacer",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Pyments");

            migrationBuilder.DropColumn(
                name: "PlatformBaqshes",
                table: "Pyments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pyments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContractID",
                table: "Pyments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
