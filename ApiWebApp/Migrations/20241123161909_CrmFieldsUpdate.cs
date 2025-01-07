using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWebApp.Migrations
{
    /// <inheritdoc />
    public partial class CrmFieldsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Employees_ContactPersonId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Tickets",
                newName: "IsOpen");

            migrationBuilder.RenameColumn(
                name: "ContactPersonId",
                table: "Tickets",
                newName: "ContactEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_ContactPersonId",
                table: "Tickets",
                newName: "IX_Tickets_ContactEmployeeId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HashTag",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Employees_ContactEmployeeId",
                table: "Tickets",
                column: "ContactEmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Employees_ContactEmployeeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "HashTag",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "IsOpen",
                table: "Tickets",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "ContactEmployeeId",
                table: "Tickets",
                newName: "ContactPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_ContactEmployeeId",
                table: "Tickets",
                newName: "IX_Tickets_ContactPersonId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tickets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Employees_ContactPersonId",
                table: "Tickets",
                column: "ContactPersonId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}
