using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexttech.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTemporaryToMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "Materials",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "Materials");
        }
    }
}
