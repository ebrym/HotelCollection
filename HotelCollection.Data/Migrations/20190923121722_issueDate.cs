using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelCollection.Data.Migrations
{
    public partial class issueDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuedBy",
                table: "RequisitionDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedBy",
                table: "RequisitionDetails");
        }
    }
}
