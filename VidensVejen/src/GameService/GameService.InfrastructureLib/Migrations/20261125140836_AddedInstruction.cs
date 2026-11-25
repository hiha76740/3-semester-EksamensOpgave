using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class AddedInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instruction",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstructionEn",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instruction",
                table: "QuizDrafts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructionEn",
                table: "QuizDrafts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instruction",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "InstructionEn",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Instruction",
                table: "QuizDrafts");

            migrationBuilder.DropColumn(
                name: "InstructionEn",
                table: "QuizDrafts");
        }
    }
}
