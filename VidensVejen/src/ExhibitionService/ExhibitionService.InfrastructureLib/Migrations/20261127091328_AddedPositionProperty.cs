using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExhibitionService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class AddedPositionProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Subjects");
        }
    }
}
