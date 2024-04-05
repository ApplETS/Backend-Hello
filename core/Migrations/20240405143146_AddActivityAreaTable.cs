using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class AddActivityAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityArea",
                table: "Organizer");

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityAreaId",
                table: "Organizer",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActivityAreaId",
                table: "Moderator",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ActivityArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameFr = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityArea", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizer_ActivityAreaId",
                table: "Organizer",
                column: "ActivityAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Moderator_ActivityAreaId",
                table: "Moderator",
                column: "ActivityAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Moderator_ActivityArea_ActivityAreaId",
                table: "Moderator",
                column: "ActivityAreaId",
                principalTable: "ActivityArea",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizer_ActivityArea_ActivityAreaId",
                table: "Organizer",
                column: "ActivityAreaId",
                principalTable: "ActivityArea",
                principalColumn: "Id");

            migrationBuilder.InsertData(
                table: "ActivityArea",
                columns: new[] { "Id", "NameFr", "NameEn", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { Guid.Parse("05ad916f-d2d0-46c1-97ac-b98ddd1bb923"), "Association étudiante", "Student Association", DateTime.UtcNow, DateTime.UtcNow },
                    { Guid.Parse("4bd96b63-f844-43e0-b486-30027e83c814"), "Centre sportif", "Sport Center", DateTime.UtcNow, DateTime.UtcNow },
                    { Guid.Parse("c1082ee9-b7bf-48c0-8a36-cb9c9b9365b1"), "Club scientifiques", "Scientific Club", DateTime.UtcNow, DateTime.UtcNow },
                    { Guid.Parse("9618693e-5a2a-4cdf-914a-5304bbaee890"), "Club sociaux", "Social Club", DateTime.UtcNow, DateTime.UtcNow },
                    { Guid.Parse("a39c7c35-aff1-4345-ad92-c2eb99cb0650"), "École de technologie supérieure", "École de technologie supérieure", DateTime.UtcNow, DateTime.UtcNow },
                    { Guid.Parse("62588aee-f9dd-4f29-bd4a-c2cbcef846b9"), "Service à la vie étudiante", "Student Life Service", DateTime.UtcNow, DateTime.UtcNow },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moderator_ActivityArea_ActivityAreaId",
                table: "Moderator");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizer_ActivityArea_ActivityAreaId",
                table: "Organizer");

            migrationBuilder.DropTable(
                name: "ActivityArea");

            migrationBuilder.DropIndex(
                name: "IX_Organizer_ActivityAreaId",
                table: "Organizer");

            migrationBuilder.DropIndex(
                name: "IX_Moderator_ActivityAreaId",
                table: "Moderator");

            migrationBuilder.DropColumn(
                name: "ActivityAreaId",
                table: "Organizer");

            migrationBuilder.DropColumn(
                name: "ActivityAreaId",
                table: "Moderator");

            migrationBuilder.AddColumn<string>(
                name: "ActivityArea",
                table: "Organizer",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
