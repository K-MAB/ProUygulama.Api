using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProUygulama.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaFileSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeaderContents_MediaFiles_BackgroundVideoId",
                table: "HeaderContents");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "MediaFiles");

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryButtonUrl",
                table: "HeaderContents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryButtonText",
                table: "HeaderContents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryButtonUrl",
                table: "HeaderContents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryButtonText",
                table: "HeaderContents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HeaderContents_MediaFiles_BackgroundVideoId",
                table: "HeaderContents",
                column: "BackgroundVideoId",
                principalTable: "MediaFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeaderContents_MediaFiles_BackgroundVideoId",
                table: "HeaderContents");

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "MediaFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryButtonUrl",
                table: "HeaderContents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SecondaryButtonText",
                table: "HeaderContents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryButtonUrl",
                table: "HeaderContents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryButtonText",
                table: "HeaderContents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_HeaderContents_MediaFiles_BackgroundVideoId",
                table: "HeaderContents",
                column: "BackgroundVideoId",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }
    }
}
