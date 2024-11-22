using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWebApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFieldsFromServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Servers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Servers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Servers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Servers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
