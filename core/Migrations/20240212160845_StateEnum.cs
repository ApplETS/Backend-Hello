using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class StateEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Publication");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Publication",
                type: "integer",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Publication");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Publication",
                type: "text",
                nullable: false);
        }
    }
}
