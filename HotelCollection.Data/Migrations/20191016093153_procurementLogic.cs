using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelCollection.Data.Migrations
{
    public partial class procurementLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcurement",
                table: "RequisitionDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "QuantityToProcure",
                table: "RequisitionDetails",
                nullable: false,
                defaultValue: 0);

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcurement",
                table: "RequisitionDetails");

            migrationBuilder.DropColumn(
                name: "QuantityToProcure",
                table: "RequisitionDetails");

     
        }
    }
}
