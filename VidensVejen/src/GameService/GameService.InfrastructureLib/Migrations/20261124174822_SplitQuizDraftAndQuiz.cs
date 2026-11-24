using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class SplitQuizDraftAndQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "CorrectFeedback",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IncorrectFeedback",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "TextEn",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QuizLayout",
                table: "Quizzes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExhibitionId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndMessageEn",
                table: "Quizzes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Quizzes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "QuizDrafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExhibitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QuizLayout = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EndMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EndMessageEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizDrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DraftQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TextEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    QuizDraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Choices = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftQuestions_QuizDrafts_QuizDraftId",
                        column: x => x.QuizDraftId,
                        principalTable: "QuizDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftQuestions_QuizDraftId",
                table: "DraftQuestions",
                column: "QuizDraftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftQuestions");

            migrationBuilder.DropTable(
                name: "QuizDrafts");

            migrationBuilder.DropColumn(
                name: "EndMessageEn",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "TextEn",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "IncorrectFeedback",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "QuizLayout",
                table: "Quizzes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExhibitionId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "CorrectFeedback",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
