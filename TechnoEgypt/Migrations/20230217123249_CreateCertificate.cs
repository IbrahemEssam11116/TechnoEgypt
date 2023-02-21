using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnoEgypt.Migrations
{
    /// <inheritdoc />
    public partial class CreateCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "childCVData",
                newName: "stationId");

            migrationBuilder.CreateTable(
                name: "ChildCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildId = table.Column<int>(type: "int", nullable: false),
                    FileURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildCourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildCertificates_childCourses_ChildCourseId",
                        column: x => x.ChildCourseId,
                        principalTable: "childCourses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChildCertificates_children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "station",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvilable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_station", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_childCVData_stationId",
                table: "childCVData",
                column: "stationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildCertificates_ChildCourseId",
                table: "ChildCertificates",
                column: "ChildCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildCertificates_ChildId",
                table: "ChildCertificates",
                column: "ChildId");

            migrationBuilder.AddForeignKey(
                name: "FK_childCVData_station_stationId",
                table: "childCVData",
                column: "stationId",
                principalTable: "station",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_childCVData_station_stationId",
                table: "childCVData");

            migrationBuilder.DropTable(
                name: "ChildCertificates");

            migrationBuilder.DropTable(
                name: "station");

            migrationBuilder.DropIndex(
                name: "IX_childCVData_stationId",
                table: "childCVData");

            migrationBuilder.RenameColumn(
                name: "stationId",
                table: "childCVData",
                newName: "Type");
        }
    }
}
