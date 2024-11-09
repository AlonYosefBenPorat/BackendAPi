using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SRc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo_Src",
                table: "Customers",
                newName: "LogoSrc");

            migrationBuilder.RenameColumn(
                name: "Logo_Alt",
                table: "Customers",
                newName: "LogoAlt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoSrc",
                table: "Customers",
                newName: "Logo_Src");

            migrationBuilder.RenameColumn(
                name: "LogoAlt",
                table: "Customers",
                newName: "Logo_Alt");
        }
    }
}
