using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class j : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildMessages_children_ChildId",
                table: "ChildMessages");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "ChildMessages",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ChildMessages_ChildId",
                table: "ChildMessages",
                newName: "IX_ChildMessages_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildMessages_Parents_ParentId",
                table: "ChildMessages",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildMessages_Parents_ParentId",
                table: "ChildMessages");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ChildMessages",
                newName: "ChildId");

            migrationBuilder.RenameIndex(
                name: "IX_ChildMessages_ParentId",
                table: "ChildMessages",
                newName: "IX_ChildMessages_ChildId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChildMessages_children_ChildId",
                table: "ChildMessages",
                column: "ChildId",
                principalTable: "children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
