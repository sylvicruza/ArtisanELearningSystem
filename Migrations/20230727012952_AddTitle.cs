using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtisanELearningSystem.Migrations
{
    public partial class AddTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Lecture",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Lecture");
        }
    }
}
