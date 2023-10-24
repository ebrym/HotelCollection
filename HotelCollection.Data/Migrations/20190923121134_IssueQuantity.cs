using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelCollection.Data.Migrations
{
    public partial class IssueQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateIssued",
                table: "RequisitionDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Issued",
                table: "RequisitionDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuantityIssued",
                table: "RequisitionDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateIssued",
                table: "RequisitionDetails");

            migrationBuilder.DropColumn(
                name: "Issued",
                table: "RequisitionDetails");

            migrationBuilder.DropColumn(
                name: "QuantityIssued",
                table: "RequisitionDetails");
        }
    }
}
