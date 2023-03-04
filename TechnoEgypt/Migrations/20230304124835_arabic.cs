using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations
{
    /// <inheritdoc />
    public partial class arabic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArDescription",
                table: "station",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArTitle",
                table: "station",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArName",
                table: "Stages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArName",
                table: "CourseToolsMyProperty",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArDescripttion",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArName",
                table: "CourseCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArDescription",
                table: "station");

            migrationBuilder.DropColumn(
                name: "ArTitle",
                table: "station");

            migrationBuilder.DropColumn(
                name: "ArName",
                table: "Stages");

            migrationBuilder.DropColumn(
                name: "ArName",
                table: "CourseToolsMyProperty");

            migrationBuilder.DropColumn(
                name: "ArDescripttion",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ArName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ArName",
                table: "CourseCategories");
        }
    }
}
