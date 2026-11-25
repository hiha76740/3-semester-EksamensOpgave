using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("227910aa-2f9f-4e37-a598-2ee40440a674"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a560a50a-a7ed-4938-8924-8707464fd16c"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1eff118c-87f2-45f6-8f1e-2a4ce1f1ccff"),
                column: "Name",
                value: "User.Read");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("389ee76d-5478-43a4-a5f9-94a9f8133eb9"),
                column: "Name",
                value: "User.Edit");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("68a14b26-15d6-4831-a876-221b01260c6b"),
                column: "Name",
                value: "User.Create");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ab46c03f-4440-4ef9-8137-9dc09f82b94c"),
                column: "Name",
                value: "User.Delete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1eff118c-87f2-45f6-8f1e-2a4ce1f1ccff"),
                column: "Name",
                value: "AuthUser.Read");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("389ee76d-5478-43a4-a5f9-94a9f8133eb9"),
                column: "Name",
                value: "AuthUser.Edit");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("68a14b26-15d6-4831-a876-221b01260c6b"),
                column: "Name",
                value: "AuthUser.Create");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("ab46c03f-4440-4ef9-8137-9dc09f82b94c"),
                column: "Name",
                value: "AuthUser.Delete");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("227910aa-2f9f-4e37-a598-2ee40440a674"), "User.Edit" },
                    { new Guid("a560a50a-a7ed-4938-8924-8707464fd16c"), "User.Read" }
                });
        }
    }
}
