using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelCollection.Data.Migrations
{
    /// <inheritdoc />
    public partial class LocalGovernmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "LocalGovernmentAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LGAName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastDateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalGovernmentAreas", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels",
                column: "CategoryId",
                principalTable: "HotelCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels");

            migrationBuilder.DropTable(
                name: "LocalGovernmentAreas");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Hotels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_HotelCategories_CategoryId",
                table: "Hotels",
                column: "CategoryId",
                principalTable: "HotelCategories",
                principalColumn: "Id");
        }
    }
}
