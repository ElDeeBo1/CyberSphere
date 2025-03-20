using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberSphere.DAL.Migrations
{
    /// <inheritdoc />
    public partial class created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CratedAt",
                table: "Books",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Books",
                newName: "CratedAt");
        }
    }
}
