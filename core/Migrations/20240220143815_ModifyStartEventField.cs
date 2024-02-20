using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class ModifyStartEventField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "Event",
                newName: "EventStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EventEndDate",
                table: "Event",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventEndDate",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "EventStartDate",
                table: "Event",
                newName: "EventDate");
        }
    }
}
