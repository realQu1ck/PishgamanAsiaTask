using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NimaTask.API.Data.NimaTaskDatabase.Migrations
{
    /// <inheritdoc />
    public partial class TokenAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NTUserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valid = table.Column<bool>(type: "bit", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTUserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NTUserTokens_NTUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "NTUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NTUserTokenLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    NTUserId = table.Column<int>(type: "int", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NTUserTokenLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NTUserTokenLogs_NTUserTokens_TokenId",
                        column: x => x.TokenId,
                        principalTable: "NTUserTokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NTUserTokenLogs_NTUsers_NTUserId",
                        column: x => x.NTUserId,
                        principalTable: "NTUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NTUserTokenLogs_NTUserId",
                table: "NTUserTokenLogs",
                column: "NTUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NTUserTokenLogs_TokenId",
                table: "NTUserTokenLogs",
                column: "TokenId");

            migrationBuilder.CreateIndex(
                name: "IX_NTUserTokens_UserId",
                table: "NTUserTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NTUserTokenLogs");

            migrationBuilder.DropTable(
                name: "NTUserTokens");
        }
    }
}
