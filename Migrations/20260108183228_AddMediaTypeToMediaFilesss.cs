using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProUygulama.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaTypeToMediaFilesss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "MediaFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "MediaFiles");
        }
    }
}
