using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class AddedRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Quizzes",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "QuizDrafts",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "QuizDrafts");
        }
    }
}
