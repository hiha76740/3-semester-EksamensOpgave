using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageReadPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("a6ae8565-c1ee-4fc9-bcba-050dbb28e3b7"), "Image.Read" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a6ae8565-c1ee-4fc9-bcba-050dbb28e3b7"));
        }
    }
}
