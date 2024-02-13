using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileDescription",
                table: "Organizer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RedditLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TikTokLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSiteLink",
                table: "Organizer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XLink",
                table: "Organizer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "FacebookLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "InstagramLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "LinkedInLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "ProfileDescription",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "RedditLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "TikTokLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "WebSiteLink",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "XLink",
                table: "Organizer");
        }
    }
}
