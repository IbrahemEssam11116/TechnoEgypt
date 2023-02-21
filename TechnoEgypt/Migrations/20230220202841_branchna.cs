using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations
{
    /// <inheritdoc />
    public partial class branchna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Branch");
        }
    }
}
