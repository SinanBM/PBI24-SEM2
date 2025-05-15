/* using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<CalculationsController> _logger;
        public CalculationsController(DatabaseContext context, ILogger<CalculationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST /api/calculations
        [HttpPost("preview")]
        public async Task<IActionResult> PreviewCalculation([FromBody] CalculationInputDto input)
        {_logger.LogInformation("Input PartsProduced: {PartsProduced}, PartMass: {PartMass}, NumberOfBuilds: {NumberOfBuilds}", input.PartsProduced, input.PartMass, input.NumberOfBuilds);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var printer = await _context.Printers.FindAsync(input.PrinterId);
            var material = await _context.Materials.FindAsync(input.MaterialId);
            if (printer == null || material == null)
                return NotFound("Printer or Material not found");

            _logger.LogError("Printer or material not found. PrinterId: {PrinterId}, MaterialId: {MaterialId}", input.PrinterId, input.MaterialId);
            _logger.LogInformation("Printer fetched: {PrinterName}, Material fetched: {MaterialName}", printer.Name, material.Name);

            // Use a helper function for cleaner logic separation
            var (materialCost, prepCost, postCost, machineUsageCost, totalConsumables, totalLabor, totalCost)
                = CalculateCosts(input, printer, material);

                _logger.LogInformation("Returning calculation result: MaterialCost: {MaterialCost}, TotalCost: {TotalCost}", materialCost, totalCost);


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
            // Save the result in the database
            [HttpPost]
            public async Task<IActionResult> SaveCalculation([FromBody] CalculationSaveDto input)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                    // Check for duplicate calc name
                bool nameExists = await _context.Calculations
                    .AnyAsync(c => c.CalcName == input.CalcName);
                if (nameExists)
                    return Conflict("A calculation with the same name already exists.");

                var printer = await _context.Printers.FindAsync(input.PrinterId);
                var material = await _context.Materials.FindAsync(input.MaterialId);
                if (printer == null || material == null)
                    return NotFound("Printer or Material not found");

                var calculation = new Calculation
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
                    MaterialCost = input.MaterialCost,
                    BuildPrepCost = input.PrepCost,
                    PostProcessCost = input.PostCost,
                    MachineCost = input.MachineCost,
                    ConsumablesCost = input.Consumables,
                    LaborCost = input.Labor,
                    TotalCost = input.TotalCost,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Calculations.Add(calculation);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCalculation), new { id = calculation.Id }, new CalculationSaveDto
                {
                    CalcName = calculation.CalcName,
                    PrinterId = calculation.PrinterId,
                    MaterialId = calculation.MaterialId,
                    PartsProduced = calculation.PartsProduced,
                    NumberOfBuilds = calculation.NumberOfBuilds,
                    PartMass = calculation.PartMass,
                    PartHeight = calculation.PartHeight,
                    PartArea = calculation.PartArea,
                    SupportMat = calculation.SupportMat,
                    MaterialCost = calculation.MaterialCost,
                    PrepCost = calculation.BuildPrepCost,
                    PostCost = calculation.PostProcessCost,
                    MachineCost = calculation.MachineCost,
                    Consumables = calculation.ConsumablesCost,
                    Labor = calculation.LaborCost,
                    TotalCost = calculation.TotalCost
                
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
            double partVolume = input.PartMass * 1000 / material.Material_density;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<CalculationSaveDto>> GetCalculation(int id)
        {
            var calc = await _context.Calculations.FindAsync(id);
            if (calc == null)
                return NotFound();

            var dto = new CalculationSaveDto
            {
                CalcName = calc.CalcName,
                PrinterId = calc.PrinterId,
                MaterialId = calc.MaterialId,
                PartsProduced = calc.PartsProduced,
                NumberOfBuilds = calc.NumberOfBuilds,
                PartMass = calc.PartMass,
                PartHeight = calc.PartHeight,
                PartArea = calc.PartArea,
                SupportMat = calc.SupportMat,
                MaterialCost = calc.MaterialCost,
                PrepCost = calc.BuildPrepCost,
                PostCost = calc.PostProcessCost,
                MachineCost = calc.MachineCost,
                Consumables = calc.ConsumablesCost,
                Labor = calc.LaborCost,
                TotalCost = calc.TotalCost
            };

            return Ok(dto);
        }
    }
}
*/