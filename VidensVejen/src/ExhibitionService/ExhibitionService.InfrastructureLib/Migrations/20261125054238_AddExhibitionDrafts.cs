using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExhibitionService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class AddExhibitionDrafts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExhibitionDrafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThemeImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExhibitionDrafts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectDrafts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExhibitionDraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopicDrafts = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectDrafts_ExhibitionDrafts_ExhibitionDraftId",
                        column: x => x.ExhibitionDraftId,
                        principalTable: "ExhibitionDrafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDrafts_ExhibitionDraftId",
                table: "SubjectDrafts",
                column: "ExhibitionDraftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectDrafts");

            migrationBuilder.DropTable(
                name: "ExhibitionDrafts");
        }
    }
}
