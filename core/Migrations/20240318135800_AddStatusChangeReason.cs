using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusChangeReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Publication",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Publication");
        }
    }
}
