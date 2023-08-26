using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtisanELearningSystem.Migrations
{
    public partial class UpdateCourseViewRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Course",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Badge",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeAgo",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalRatings",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseRating_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRating_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseRating_CourseId",
                table: "CourseRating",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRating_StudentId",
                table: "CourseRating",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseRating");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Badge",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Poster",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TimeAgo",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TotalRatings",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Course");
        }
    }
}
