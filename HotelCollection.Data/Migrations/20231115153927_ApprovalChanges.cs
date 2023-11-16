using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelCollection.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApprovalChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequisitionId",
                table: "Approvals",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "ApprovalConfigs",
                newName: "PaymentType");

            migrationBuilder.AddColumn<string>(
                name: "ApprovalStatus",
                table: "PaymentSetups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentApprovalLevel",
                table: "PaymentSetups",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "PaymentSetups");

            migrationBuilder.DropColumn(
                name: "CurrentApprovalLevel",
                table: "PaymentSetups");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Approvals",
                newName: "RequisitionId");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "ApprovalConfigs",
                newName: "Department");
        }
    }
}
