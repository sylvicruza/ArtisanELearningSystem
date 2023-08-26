using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtisanELearningSystem.Migrations
{
    public partial class UpdateCourseLectureQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Curriculum_CurriculumId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Curriculum_CurriculumId",
                table: "Section");

            migrationBuilder.DropTable(
                name: "Curriculum");

            migrationBuilder.DropIndex(
                name: "IX_Section_CurriculumId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Course_CurriculumId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CurriculumId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "CurriculumId",
                table: "Course");



            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Lecture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoginRequired",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);



            migrationBuilder.CreateIndex(
                name: "IX_Lecture_CourseId",
                table: "Lecture",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Course_CourseId",
                table: "Lecture",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Course_CourseId",
                table: "Lecture");

           

            migrationBuilder.DropIndex(
                name: "IX_Lecture_CourseId",
                table: "Lecture");

           
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Lecture");

            migrationBuilder.AddColumn<int>(
                name: "CurriculumId",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoginRequired",
                table: "Course",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "CurriculumId",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Curriculum",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculum", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Section_CurriculumId",
                table: "Section",
                column: "CurriculumId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_CurriculumId",
                table: "Course",
                column: "CurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Curriculum_CurriculumId",
                table: "Course",
                column: "CurriculumId",
                principalTable: "Curriculum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Curriculum_CurriculumId",
                table: "Section",
                column: "CurriculumId",
                principalTable: "Curriculum",
                principalColumn: "Id");
        }
    }
}
