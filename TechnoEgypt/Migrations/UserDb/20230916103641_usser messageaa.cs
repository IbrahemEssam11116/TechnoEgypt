using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class ussermessageaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedUserId",
                table: "ChildMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ChildMessages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ChildMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChildMessages_CreatedUserId",
                table: "ChildMessages",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildMessages_AspNetUsers_CreatedUserId",
                table: "ChildMessages",
                column: "CreatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildMessages_AspNetUsers_CreatedUserId",
                table: "ChildMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChildMessages_CreatedUserId",
                table: "ChildMessages");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "ChildMessages");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ChildMessages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ChildMessages");
        }
    }
}
