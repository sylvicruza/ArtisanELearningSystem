using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtisanELearningSystem.Migrations
{
    public partial class AddedInstructorToLecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Lecture",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_InstructorId",
                table: "Lecture",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Instructor_InstructorId",
                table: "Lecture",
                column: "InstructorId",
                principalTable: "Instructor",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Instructor_InstructorId",
                table: "Lecture");

            migrationBuilder.DropIndex(
                name: "IX_Lecture_InstructorId",
                table: "Lecture");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Lecture");
        }
    }
}
