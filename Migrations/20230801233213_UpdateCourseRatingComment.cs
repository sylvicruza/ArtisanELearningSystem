using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtisanELearningSystem.Migrations
{
    public partial class UpdateCourseRatingComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "CourseRating",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "CourseRating");
        }
    }
}
