using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExhibitionService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnglishProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "Exhibitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "Exhibitions");
        }
    }
}
