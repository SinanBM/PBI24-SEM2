using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexttech.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImageToCalculations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CalcName",
                table: "Calculations",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "Calculations",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Calculations");

            migrationBuilder.UpdateData(
                table: "Calculations",
                keyColumn: "CalcName",
                keyValue: null,
                column: "CalcName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CalcName",
                table: "Calculations",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
