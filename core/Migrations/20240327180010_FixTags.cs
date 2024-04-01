using api.core.Migrations.SeededData;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.core.Migrations
{
    /// <inheritdoc />
    public partial class FixTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < TagSeed0Initial.TagValues.GetLength(0); i++)
            {
                Guid id = (Guid)TagSeed0Initial.TagValues[i, 0];
                migrationBuilder.DeleteData("Tag", "Id", id);
            }

            migrationBuilder.InsertData("Tag", TagSeed20240327180010.Columns, TagSeed20240327180010.TagValues);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < TagSeed20240327180010.TagValues.GetLength(0); i++)
            {
                Guid id = (Guid)TagSeed20240327180010.TagValues[i, 0];
                migrationBuilder.DeleteData("Tag", "Id", id);
            }
            migrationBuilder.InsertData("Tag", TagSeed0Initial.Columns, TagSeed0Initial.TagValues);
        }
    }
}
