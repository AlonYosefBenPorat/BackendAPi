using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWebApp.Migrations
{
    /// <inheritdoc />
    public partial class ProfileImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "AspNetUsers",
                newName: "ProfileImage_Src");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage_Alt",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage_Alt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfileImage_Src",
                table: "AspNetUsers",
                newName: "ProfileImage");
        }
    }
}
