using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations
{
    /// <inheritdoc />
    public partial class branch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Parents",
                newName: "HomePhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Parents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FatherEmail",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherPhoneNumber",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FatherTitle",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherEmail",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherPhoneNumber",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherTitle",
                table: "Parents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Parents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CognitiveAbilities",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CriticalThinking",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DataCollectionandAnalysis",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Innovation",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LogicalThinking",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MathematicalReasoning",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Planning",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProblemSolving",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ScientificResearch",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SocialLifeSkills",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "children",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parents_BranchId",
                table: "Parents",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Branch_BranchId",
                table: "Parents",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Branch_BranchId",
                table: "Parents");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropIndex(
                name: "IX_Parents_BranchId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "FatherEmail",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "FatherPhoneNumber",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "FatherTitle",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "MotherEmail",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "MotherPhoneNumber",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "MotherTitle",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "CognitiveAbilities",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CriticalThinking",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "DataCollectionandAnalysis",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Innovation",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LogicalThinking",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MathematicalReasoning",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Planning",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ProblemSolving",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ScientificResearch",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SocialLifeSkills",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "children");

            migrationBuilder.RenameColumn(
                name: "HomePhoneNumber",
                table: "Parents",
                newName: "PhoneNumber");
        }
    }
}
