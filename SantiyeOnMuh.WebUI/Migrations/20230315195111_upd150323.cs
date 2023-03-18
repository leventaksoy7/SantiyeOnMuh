using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantiyeOnMuh.WebUI.Migrations
{
    /// <inheritdoc />
    public partial class upd150323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SantiyeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SantiyeId",
                table: "AspNetUsers");
        }
    }
}
