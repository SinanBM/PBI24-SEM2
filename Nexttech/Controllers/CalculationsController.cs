using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexttech.Data;
using Nexttech.Models;

namespace Nexttech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculationsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CalculationsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CalculateAndSave([FromBody] CalculationInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var printer = await _context.Printers.FindAsync(input.PrinterId);
            var material = await _context.Materials.FindAsync(input.MaterialId);
            if (printer == null || material == null)
                return NotFound("Printer or Material not found");

            // Use a helper function for cleaner logic separation
            var (materialCost, prepCost, postCost, machineUsageCost, totalConsumables, totalLabor, totalCost)
                = CalculateCosts(input, printer, material);

            // Save the result in the database
            var result = new Calculation
            {
                CalcName = input.CalcName,
                PrinterId = input.PrinterId,
                MaterialId = input.MaterialId,
                PartsProduced = input.PartsProduced,
                NumberOfBuilds = input.NumberOfBuilds,
                PartMass = input.PartMass,
                PartHeight = input.PartHeight,
                PartArea = input.PartArea,
                SupportMat = input.SupportMat,
                MaterialCost = materialCost,
                BuildPrepCost = prepCost,
                PostProcessCost = postCost,
                MachineCost = machineUsageCost,
                ConsumablesCost = totalConsumables,
                LaborCost = totalLabor,
                TotalCost = totalCost,
                CreatedAt = DateTime.UtcNow
            };

            _context.Calculations.Add(result);
            await _context.SaveChangesAsync();

            return Ok(new CalculationResultDto
            {
                MaterialCost = materialCost,
                PrepCost = prepCost,
                PostCost = postCost,
                MachineCost = machineUsageCost,
                Consumables = totalConsumables,
                Labor = totalLabor,
                TotalCost = totalCost
            });
        }

        // Helper method to calculate costs
        private static (double materialCost, double prepCost, double postCost, double machineUsageCost, double totalConsumables, double totalLabor, double totalCost)
            CalculateCosts(CalculationInputDto input, Printer printer, Material material)
        {
            // Extract inputs
            double totalMaterial = input.PartsProduced * input.PartMass;
            double supportMass = input.SupportMat * input.PartMass;
            double totalSupport = input.PartsProduced * (input.SupportMat * supportMass);
            double totalMaterialAllBuilds = input.NumberOfBuilds * printer.Machine_Build_Area * printer.Machine_Build_Height * material.Material_density / 1000;
            double recycled = (totalMaterialAllBuilds - totalMaterial - totalSupport) * printer.Recycling_fraction;
            double waste = totalMaterialAllBuilds - recycled - totalMaterial - totalSupport;
            double required = waste + totalMaterial + totalSupport;
            double materialCost = required * material.Material_cost;

            double prepTime = ((input.NumberOfBuilds - 1) * printer.Subsequent_build_preparation) + printer.First_time_build_preparation;
            double prepCost = prepTime * printer.FTE_salary_engineer;

            double postTimePerPart = printer.Support_removal_time_labor_constant * Math.Sqrt(input.PartArea);
            double totalPostTime = postTimePerPart * input.PartsProduced;
            double postCost = totalPostTime * printer.FTE_salary_technician;

            double upFront = printer.Purchase_cost * (1 + printer.Infrastructure_Cost);
            double annualDep = printer.Cost_Of_Capital * upFront / (1 - Math.Pow(1 + printer.Cost_Of_Capital, -printer.Machine_lifetime));
            double annualMaint = printer.Maintenance * printer.Purchase_cost;
            double annualMachine = annualDep + annualMaint;
            double hoursPerYear = printer.Hours_per_day * printer.Days_per_week * 52 * printer.Machine_Uptime;
            double machineCostHour = annualMachine / hoursPerYear;

            double warmupTotal = input.NumberOfBuilds * printer.Time_per_machine_warm_up;
            double partVolume = (input.PartMass * 1000) / material.Material_density;
            double printTime = input.PartsProduced * partVolume / printer.Machine_Build_Rate;
            double cooldownTotal = input.NumberOfBuilds * printer.Time_per_machine_cool_down;
            double exchangeTime = input.NumberOfBuilds * (printer.Time_per_build_setup + printer.Time_per_build_removal);
            double machineTime = warmupTotal + printTime + cooldownTotal + exchangeTime;
            double machineUsageCost = machineTime * machineCostHour;

            double opTotalCost = printer.Additional_operating_cost * (warmupTotal + printTime + cooldownTotal);
            double consumables = printer.Consumable_cost_per_build * input.NumberOfBuilds;
            double totalConsumables = opTotalCost + consumables;

            double laborBuild = (warmupTotal + printTime + cooldownTotal) * printer.FTE_per_machine_supervised;
            double laborExchange = exchangeTime * printer.FTE_for_build_exchange;
            double totalLabor = (laborBuild + laborExchange) * printer.FTE_salary_operator;

            double totalCost = materialCost + prepCost + postCost + machineUsageCost + totalConsumables + totalLabor;

            // Return the tuple with all calculated costs
            return (materialCost, prepCost, postCost, machineUsageCost, totalConsumables, totalLabor, totalCost);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calculation>>> GetCalculations()
        {
            return await _context.Calculations
                .Include(c => c.Printer)
                .Include(c => c.Material)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
