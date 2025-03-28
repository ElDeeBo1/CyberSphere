using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberSphere.DAL.Migrations
{
    /// <inheritdoc />
    public partial class pro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "ProgressId",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Progresss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    LessonId = table.Column<int>(type: "int", nullable: true),
                    LessonsCompleted = table.Column<int>(type: "int", nullable: false),
                    TotalLessons = table.Column<int>(type: "int", nullable: false),
                    CompletionPercentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progresss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Progresss_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Progresss_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Progresss_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ProgressId",
                table: "Certificates",
                column: "ProgressId");

            migrationBuilder.CreateIndex(
                name: "IX_Progresss_CourseId",
                table: "Progresss",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Progresss_LessonId",
                table: "Progresss",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Progresss_StudentId",
                table: "Progresss",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Progresss_ProgressId",
                table: "Certificates",
                column: "ProgressId",
                principalTable: "Progresss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Progresss_ProgressId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "Progresss");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_ProgressId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "ProgressId",
                table: "Certificates");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
