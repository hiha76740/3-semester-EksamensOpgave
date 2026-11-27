using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.InfrastructureLib.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecievedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProccessedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthUserAccessPermissions",
                columns: table => new
                {
                    AuthUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUserAccessPermissions", x => new { x.AuthUserId, x.PermissionsId });
                    table.ForeignKey(
                        name: "FK_AuthUserAccessPermissions_AuthUsers_AuthUserId",
                        column: x => x.AuthUserId,
                        principalTable: "AuthUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthUserAccessPermissions_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0fb02b10-29ea-4616-83d8-5e7870e24c3f"), "Exhibition.Delete" },
                    { new Guid("1d4d28b2-edc2-4aaa-be9e-e855f4dd7cd3"), "Exhibition.Edit" },
                    { new Guid("1eff118c-87f2-45f6-8f1e-2a4ce1f1ccff"), "User.Read" },
                    { new Guid("35ae0b5a-36cf-4242-a7f3-f10a616e6626"), "Game.Publish" },
                    { new Guid("389ee76d-5478-43a4-a5f9-94a9f8133eb9"), "User.Edit" },
                    { new Guid("420dedfe-0bed-40ab-b036-63fbe52fbbab"), "Exhibition.Read" },
                    { new Guid("577779d0-f015-4827-8535-0318210db489"), "Game.Delete" },
                    { new Guid("68a14b26-15d6-4831-a876-221b01260c6b"), "User.Create" },
                    { new Guid("a5d70bf0-67d1-4a14-bf0b-2857d3307e11"), "Game.Edit" },
                    { new Guid("ab46c03f-4440-4ef9-8137-9dc09f82b94c"), "User.Delete" },
                    { new Guid("e48eb056-2c92-4642-90a9-dd685edb7222"), "Image.Upload" },
                    { new Guid("efcc418e-1113-4d82-b67e-3f6efc06765d"), "Game.Read" },
                    { new Guid("f595c2a6-13b0-438c-bc63-c08fff41ddc4"), "Exhibition.Publish" },
                    { new Guid("f5ad63ca-85e0-40f8-9315-f6f7cd088408"), "Image.Delete" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthUserAccessPermissions_PermissionsId",
                table: "AuthUserAccessPermissions",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthUsers_Email",
                table: "AuthUsers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthUserAccessPermissions");

            migrationBuilder.DropTable(
                name: "Inbox");

            migrationBuilder.DropTable(
                name: "Outbox");

            migrationBuilder.DropTable(
                name: "AuthUsers");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
