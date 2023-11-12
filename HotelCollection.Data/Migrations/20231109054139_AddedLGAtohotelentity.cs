using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelCollection.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLGAtohotelentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LGAId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocalGovernmentAreaId",
                table: "Hotels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_LocalGovernmentAreaId",
                table: "Hotels",
                column: "LocalGovernmentAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_LocalGovernmentAreas_LocalGovernmentAreaId",
                table: "Hotels",
                column: "LocalGovernmentAreaId",
                principalTable: "LocalGovernmentAreas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_LocalGovernmentAreas_LocalGovernmentAreaId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_LocalGovernmentAreaId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "LGAId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "LocalGovernmentAreaId",
                table: "Hotels");
        }
    }
}
