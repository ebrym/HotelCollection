using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelCollection.Data.Migrations
{
    /// <inheritdoc />
    public partial class Payment_Setup_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_HotelCategories_CategoryId",
                table: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTypes_CategoryId",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PaymentTypes");

            migrationBuilder.CreateTable(
                name: "PaymentSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    ReferenceNo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastDateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentSetups_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentSetups_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSetups_HotelId",
                table: "PaymentSetups",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSetups_PaymentTypeId",
                table: "PaymentSetups",
                column: "PaymentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentSetups");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "PaymentTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_CategoryId",
                table: "PaymentTypes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_HotelCategories_CategoryId",
                table: "PaymentTypes",
                column: "CategoryId",
                principalTable: "HotelCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
