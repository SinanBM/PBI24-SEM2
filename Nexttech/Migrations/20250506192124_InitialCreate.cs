using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexttech.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Material_cost = table.Column<double>(type: "double", nullable: false),
                    Material_density = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Purchase_cost = table.Column<double>(type: "double", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Machine_lifetime = table.Column<double>(type: "double", nullable: false),
                    Cost_Of_Capital = table.Column<double>(type: "double", nullable: false),
                    Infrastructure_Cost = table.Column<double>(type: "double", nullable: false),
                    Maintenance = table.Column<double>(type: "double", nullable: false),
                    Machine_Build_Area = table.Column<double>(type: "double", nullable: false),
                    Machine_Build_Height = table.Column<double>(type: "double", nullable: false),
                    Machine_Build_Volume = table.Column<double>(type: "double", nullable: false),
                    Machine_Build_Rate = table.Column<double>(type: "double", nullable: false),
                    Machine_Uptime = table.Column<double>(type: "double", nullable: false),
                    Packing_policy = table.Column<int>(type: "int", nullable: false),
                    Packing_fraction = table.Column<double>(type: "double", nullable: false),
                    Recycling_fraction = table.Column<double>(type: "double", nullable: false),
                    Additional_operating_cost = table.Column<double>(type: "double", nullable: false),
                    Consumable_cost_per_build = table.Column<double>(type: "double", nullable: false),
                    First_time_build_preparation = table.Column<double>(type: "double", nullable: false),
                    Subsequent_build_preparation = table.Column<double>(type: "double", nullable: false),
                    Time_per_build_setup = table.Column<double>(type: "double", nullable: false),
                    Time_per_build_removal = table.Column<double>(type: "double", nullable: false),
                    Time_per_machine_warm_up = table.Column<double>(type: "double", nullable: false),
                    Time_per_machine_cool_down = table.Column<double>(type: "double", nullable: false),
                    Support_removal_time_labor_constant = table.Column<double>(type: "double", nullable: false),
                    Hours_per_day = table.Column<double>(type: "double", nullable: false),
                    Days_per_week = table.Column<double>(type: "double", nullable: false),
                    FTE_per_machine_supervised = table.Column<double>(type: "double", nullable: false),
                    FTE_for_build_exchange = table.Column<double>(type: "double", nullable: false),
                    FTE_for_support_removal = table.Column<double>(type: "double", nullable: false),
                    FTE_salary_engineer = table.Column<double>(type: "double", nullable: false),
                    FTE_salary_operator = table.Column<double>(type: "double", nullable: false),
                    FTE_salary_technician = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Printers");
        }
    }
}
