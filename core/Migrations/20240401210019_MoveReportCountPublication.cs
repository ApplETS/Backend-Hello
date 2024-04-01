using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class MoveReportCountPublication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportCount",
                table: "Event");

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenReported",
                table: "Publication",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportCount",
                table: "Publication",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenReported",
                table: "Publication");

            migrationBuilder.DropColumn(
                name: "ReportCount",
                table: "Publication");

            migrationBuilder.AddColumn<int>(
                name: "ReportCount",
                table: "Event",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
