using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations
{
    /// <inheritdoc />
    public partial class addCourseFalidDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "Courses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "Courses",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "Courses");
        }
    }
}
