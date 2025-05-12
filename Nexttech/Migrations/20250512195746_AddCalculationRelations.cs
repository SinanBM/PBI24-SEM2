using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexttech.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculationRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CalcName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrinterId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    PartsProduced = table.Column<int>(type: "int", nullable: false),
                    NumberOfBuilds = table.Column<int>(type: "int", nullable: false),
                    PartMass = table.Column<double>(type: "double", nullable: false),
                    PartHeight = table.Column<double>(type: "double", nullable: false),
                    PartArea = table.Column<double>(type: "double", nullable: false),
                    SupportMat = table.Column<double>(type: "double", nullable: false),
                    MaterialCost = table.Column<double>(type: "double", nullable: false),
                    BuildPrepCost = table.Column<double>(type: "double", nullable: false),
                    PostProcessCost = table.Column<double>(type: "double", nullable: false),
                    MachineCost = table.Column<double>(type: "double", nullable: false),
                    ConsumablesCost = table.Column<double>(type: "double", nullable: false),
                    LaborCost = table.Column<double>(type: "double", nullable: false),
                    TotalCost = table.Column<double>(type: "double", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calculations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Calculations_Printers_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "Printers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Calculations_MaterialId",
                table: "Calculations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Calculations_PrinterId",
                table: "Calculations",
                column: "PrinterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calculations");
        }
    }
}
