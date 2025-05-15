using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexttech.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailedCalculationOutputs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnnualDepreciation",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AnnualMachineCost",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AnnualMaintenance",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Consumables",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CooldownTotal",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExchangeTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HoursPerYear",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaborBuildTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LaborExchangeTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MachineCostPerHour",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MachineTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperatingTotalCost",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PartVolume",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PostTimePerPart",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PrepTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PrintTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Recycled",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RequiredMaterial",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SupportMass",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalMaterial",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalMaterialAllBuilds",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPostTime",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalSupport",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UpFront",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WarmupTotal",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Waste",
                table: "Calculations",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualDepreciation",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "AnnualMachineCost",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "AnnualMaintenance",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "Consumables",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "CooldownTotal",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "ExchangeTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "HoursPerYear",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "LaborBuildTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "LaborExchangeTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "MachineCostPerHour",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "MachineTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "OperatingTotalCost",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "PartVolume",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "PostTimePerPart",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "PrepTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "PrintTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "Recycled",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "RequiredMaterial",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "SupportMass",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "TotalMaterial",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "TotalMaterialAllBuilds",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "TotalPostTime",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "TotalSupport",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "UpFront",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "WarmupTotal",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "Waste",
                table: "Calculations");
        }
    }
}
