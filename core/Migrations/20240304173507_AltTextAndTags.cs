using api.core.Migrations.SeededData;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class AltTextAndTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_thumbnail",
                table: "Publication");

            migrationBuilder.AddColumn<string>(
                name: "ImageAltText",
                table: "Publication",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData("Tag", TagSeed.Columns, TagSeed.TagValues);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAltText",
                table: "Publication");

            migrationBuilder.AddColumn<byte[]>(
                name: "image_thumbnail",
                table: "Publication",
                type: "BYTEA",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
