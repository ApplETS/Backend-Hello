using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class UserMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Publication_ModeratorId_fkey",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "Publication_OrganizerId_fkey",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Organizer_OrganizerId",
                table: "Subscription");

            migrationBuilder.DropTable(
                name: "Moderator");

            migrationBuilder.DropTable(
                name: "Organizer");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Subscription",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Publication",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "ModeratorId",
                table: "Publication",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ActivityAreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Organization = table.Column<string>(type: "text", nullable: false),
                    ProfileDescription = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    HasLoggedIn = table.Column<bool>(type: "boolean", nullable: false),
                    FacebookLink = table.Column<string>(type: "text", nullable: true),
                    InstagramLink = table.Column<string>(type: "text", nullable: true),
                    TikTokLink = table.Column<string>(type: "text", nullable: true),
                    XLink = table.Column<string>(type: "text", nullable: true),
                    DiscordLink = table.Column<string>(type: "text", nullable: true),
                    LinkedInLink = table.Column<string>(type: "text", nullable: true),
                    RedditLink = table.Column<string>(type: "text", nullable: true),
                    WebSiteLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_ActivityArea_ActivityAreaId",
                        column: x => x.ActivityAreaId,
                        principalTable: "ActivityArea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ActivityAreaId",
                table: "User",
                column: "ActivityAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_User_ModeratorId",
                table: "Publication",
                column: "ModeratorId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publication_User_OrganizerId",
                table: "Publication",
                column: "OrganizerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_User_OrganizerId",
                table: "Subscription",
                column: "OrganizerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publication_User_ModeratorId",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "FK_Publication_User_OrganizerId",
                table: "Publication");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_User_OrganizerId",
                table: "Subscription");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizerId",
                table: "Subscription",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizerId",
                table: "Publication",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModeratorId",
                table: "Publication",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Moderator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ActivityAreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moderator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moderator_ActivityArea_ActivityAreaId",
                        column: x => x.ActivityAreaId,
                        principalTable: "ActivityArea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Organizer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ActivityAreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)"),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DiscordLink = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FacebookLink = table.Column<string>(type: "text", nullable: true),
                    HasLoggedIn = table.Column<bool>(type: "boolean", nullable: false),
                    InstagramLink = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LinkedInLink = table.Column<string>(type: "text", nullable: true),
                    Organization = table.Column<string>(type: "text", nullable: false),
                    ProfileDescription = table.Column<string>(type: "text", nullable: false),
                    RedditLink = table.Column<string>(type: "text", nullable: true),
                    TikTokLink = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "(now() AT TIME ZONE 'utc'::text)"),
                    WebSiteLink = table.Column<string>(type: "text", nullable: true),
                    XLink = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizer_ActivityArea_ActivityAreaId",
                        column: x => x.ActivityAreaId,
                        principalTable: "ActivityArea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moderator_ActivityAreaId",
                table: "Moderator",
                column: "ActivityAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizer_ActivityAreaId",
                table: "Organizer",
                column: "ActivityAreaId");

            migrationBuilder.AddForeignKey(
                name: "Publication_ModeratorId_fkey",
                table: "Publication",
                column: "ModeratorId",
                principalTable: "Moderator",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Publication_OrganizerId_fkey",
                table: "Publication",
                column: "OrganizerId",
                principalTable: "Organizer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Organizer_OrganizerId",
                table: "Subscription",
                column: "OrganizerId",
                principalTable: "Organizer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
