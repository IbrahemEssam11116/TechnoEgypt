using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_childCVData_station_stationId",
                table: "childCVData");

            migrationBuilder.DropTable(
                name: "station");

            migrationBuilder.DropIndex(
                name: "IX_childCVData_stationId",
                table: "childCVData");

            migrationBuilder.CreateTable(
                name: "childPersonalStatements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_childPersonalStatements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_childPersonalStatements_children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "childSchoolReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_childSchoolReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_childSchoolReports_children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_childPersonalStatements_ChildId",
                table: "childPersonalStatements",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_childSchoolReports_ChildId",
                table: "childSchoolReports",
                column: "ChildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "childPersonalStatements");

            migrationBuilder.DropTable(
                name: "childSchoolReports");

            migrationBuilder.CreateTable(
                name: "station",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvilable = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_station", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_childCVData_stationId",
                table: "childCVData",
                column: "stationId");

            migrationBuilder.AddForeignKey(
                name: "FK_childCVData_station_stationId",
                table: "childCVData",
                column: "stationId",
                principalTable: "station",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
