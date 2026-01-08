using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProUygulama.Api.Migrations
{
    /// <inheritdoc />
    public partial class Add_HeaderContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeaderContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    Slogan = table.Column<string>(type: "text", nullable: false),
                    PrimaryButtonText = table.Column<string>(type: "text", nullable: false),
                    PrimaryButtonUrl = table.Column<string>(type: "text", nullable: false),
                    SecondaryButtonText = table.Column<string>(type: "text", nullable: true),
                    SecondaryButtonUrl = table.Column<string>(type: "text", nullable: true),
                    BackgroundVideoId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeaderContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeaderContents_MediaFiles_BackgroundVideoId",
                        column: x => x.BackgroundVideoId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeaderContents_BackgroundVideoId",
                table: "HeaderContents",
                column: "BackgroundVideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeaderContents");
        }
    }
}
