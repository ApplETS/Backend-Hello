using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivityAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityArea",
                keyColumn: "Id",
                keyValue: Guid.Parse("c1082ee9-b7bf-48c0-8a36-cb9c9b9365b1"),
                column: "NameFr",
                value: "Club scientifique");

            migrationBuilder.UpdateData(
                table: "ActivityArea",
                keyColumn: "Id",
                keyValue: Guid.Parse("9618693e-5a2a-4cdf-914a-5304bbaee890"),
                column: "NameFr",
                value: "Club social");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActivityArea",
                keyColumn: "Id",
                keyValue: Guid.Parse("c1082ee9-b7bf-48c0-8a36-cb9c9b9365b1"),
                column: "NameFr",
                value: "Club scientifiques");

            migrationBuilder.UpdateData(
                table: "ActivityArea",
                keyColumn: "Id",
                keyValue: Guid.Parse("9618693e-5a2a-4cdf-914a-5304bbaee890"),
                column: "NameFr",
                value: "Club sociaux");
        }
    }
}
