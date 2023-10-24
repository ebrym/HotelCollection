using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelCollection.Data.Migrations
{
    public partial class RequestUserDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Requisitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Requisitions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Requisitions");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Requisitions");
        }
    }
}
