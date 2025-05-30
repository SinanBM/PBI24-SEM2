using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexttech.Data;
using Nexttech.Models;
using Nexttech.Services;

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

        // POST /api/calculations/preview
        [HttpPost("preview")]
        public async Task<IActionResult> PreviewCalculation([FromBody] CalculationInputDto input)
        {
            _logger.LogInformation("Input PartsProduced: {PartsProduced}, PartMass: {PartMass}, NumberOfBuilds: {NumberOfBuilds}",
                input.PartsProduced, input.PartMass, input.NumberOfBuilds);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var printer = await _context.Printers.FindAsync(input.PrinterId);
            if (printer == null)
            {
                _logger.LogError("Printer not found. PrinterId: {PrinterId}", input.PrinterId);
                return NotFound("Printer not found");
            }

            var material = await _context.Materials.FindAsync(input.MaterialId);
            if (material == null)
            {
                _logger.LogError("Material not found. MaterialId: {MaterialId}", input.MaterialId);
                return NotFound("Material not found");
            }
            _logger.LogInformation("Material fetched: {MaterialName}", material.Name);


            _logger.LogInformation("Printer fetched: {PrinterName}", printer.Name);

            var calculation = GenerateFullCalculation(input, printer, material);

            _logger.LogInformation("Returning calculation result: MaterialCost: {MaterialCost}, TotalCost: {TotalCost}",
                calculation.MaterialCost, calculation.TotalCost);

            // Delete all temporary materials after using the current calculation
            var tempMaterials = _context.Materials.Where(m => m.IsTemporary);
            _context.Materials.RemoveRange(tempMaterials);
            await _context.SaveChangesAsync();

            _logger.LogInformation("All temporary materials deleted.");

            return Ok(new CalculationResultDto
            {
                MaterialCost = calculation.MaterialCost,
                BuildPrepCost = calculation.BuildPrepCost,
                PostProcessCost = calculation.PostProcessCost,
                MachineCost = calculation.MachineCost,
                Consumables = calculation.ConsumablesCost,
                Labor = calculation.LaborCost,
                TotalCost = calculation.TotalCost
            });
        }


        // POST /api/calculations
        [HttpPost]
        public async Task<IActionResult> SaveCalculation([FromBody] CalculationInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool nameExists = await _context.Calculations.AnyAsync(c => c.CalcName == input.CalcName);
            if (nameExists)
                return Conflict("A calculation with the same name already exists.");

            var printer = await _context.Printers.FindAsync(input.PrinterId);
            var material = await _context.Materials.FindAsync(input.MaterialId);



            if (printer == null || material == null)
                return NotFound("Printer or Material not found");

            var calculation = GenerateFullCalculation(input, printer, material);

            var calculationSaveDto = new CalculationSaveDto
            {
                // Map input
                CalcName = input.CalcName,
                PrinterId = input.PrinterId,
                MaterialId = input.MaterialId,
                PartsProduced = input.PartsProduced,
                NumberOfBuilds = input.NumberOfBuilds,
                PartMass = input.PartMass!.Value,
                PartHeight = input.PartHeight!.Value,
                PartArea = input.PartArea!.Value,
                SupportMat = input.SupportMat,

                // Map calculated results
                TotalMaterial = calculation.TotalMaterial,
                SupportMass = calculation.SupportMass,
                TotalSupport = calculation.TotalSupport,
                TotalMaterialAllBuilds = calculation.TotalMaterialAllBuilds,
                Recycled = calculation.Recycled,
                Waste = calculation.Waste,
                RequiredMaterial = calculation.RequiredMaterial,
                MaterialCost = calculation.MaterialCost,
                PrepTime = calculation.PrepTime,
                BuildPrepCost = calculation.BuildPrepCost,
                PostTimePerPart = calculation.PostTimePerPart,
                TotalPostTime = calculation.TotalPostTime,
                PostProcessCost = calculation.PostProcessCost,
                UpFront = calculation.UpFront,
                AnnualDepreciation = calculation.AnnualDepreciation,
                AnnualMaintenance = calculation.AnnualMaintenance,
                AnnualMachineCost = calculation.AnnualMachineCost,
                HoursPerYear = calculation.HoursPerYear,
                MachineCostPerHour = calculation.MachineCostPerHour,
                WarmupTotal = calculation.WarmupTotal,
                PartVolume = calculation.PartVolume,
                PrintTime = calculation.PrintTime,
                CooldownTotal = calculation.CooldownTotal,
                ExchangeTime = calculation.ExchangeTime,
                MachineTime = calculation.MachineTime,
                MachineCost = calculation.MachineCost,
                OperatingTotalCost = calculation.OperatingTotalCost,
                Consumables = calculation.Consumables,
                ConsumablesCost = calculation.ConsumablesCost,
                LaborBuildTime = calculation.LaborBuildTime,
                LaborExchangeTime = calculation.LaborExchangeTime,
                LaborCost = calculation.LaborCost,
                TotalCost = calculation.TotalCost,

                CreatedAt = DateTime.UtcNow
            };

            var calculationToSave = new Calculation
            {
                CalcName = calculationSaveDto.CalcName,
                PrinterId = calculationSaveDto.PrinterId,
                MaterialId = calculationSaveDto.MaterialId,
                PartsProduced = calculationSaveDto.PartsProduced,
                NumberOfBuilds = calculationSaveDto.NumberOfBuilds,
                PartMass = calculationSaveDto.PartMass,
                PartHeight = calculationSaveDto.PartHeight,
                PartArea = calculationSaveDto.PartArea,
                SupportMat = calculationSaveDto.SupportMat,

                MaterialCost = calculationSaveDto.MaterialCost,
                BuildPrepCost = calculationSaveDto.BuildPrepCost,
                PostProcessCost = calculationSaveDto.PostProcessCost,
                MachineCost = calculationSaveDto.MachineCost,
                ConsumablesCost = calculationSaveDto.ConsumablesCost,
                LaborCost = calculationSaveDto.LaborCost,
                TotalCost = calculationSaveDto.TotalCost,

                // Add all other properties here too:
                SupportMass = calculationSaveDto.SupportMass,
                TotalSupport = calculationSaveDto.TotalSupport,
                PrepTime = calculationSaveDto.PrepTime,
                PostTimePerPart = calculationSaveDto.PostTimePerPart,
                TotalPostTime = calculationSaveDto.TotalPostTime,
                UpFront = calculationSaveDto.UpFront,
                AnnualDepreciation = calculationSaveDto.AnnualDepreciation,
                AnnualMaintenance = calculationSaveDto.AnnualMaintenance,
                AnnualMachineCost = calculationSaveDto.AnnualMachineCost,
                HoursPerYear = calculationSaveDto.HoursPerYear,
                MachineCostPerHour = calculationSaveDto.MachineCostPerHour,
                WarmupTotal = calculationSaveDto.WarmupTotal,
                PartVolume = calculationSaveDto.PartVolume,
                PrintTime = calculationSaveDto.PrintTime,
                CooldownTotal = calculationSaveDto.CooldownTotal,
                ExchangeTime = calculationSaveDto.ExchangeTime,
                MachineTime = calculationSaveDto.MachineTime,
                OperatingTotalCost = calculationSaveDto.OperatingTotalCost,
                Consumables = calculationSaveDto.Consumables,
                LaborBuildTime = calculationSaveDto.LaborBuildTime,
                LaborExchangeTime = calculationSaveDto.LaborExchangeTime,

                CreatedAt = calculationSaveDto.CreatedAt
            };

            _context.Calculations.Add(calculationToSave);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCalculation), new { id = calculationToSave.Id }, calculationSaveDto);
        }

        // Helper method for full calculation
        protected Calculation GenerateFullCalculation(CalculationInputDto input, Printer printer, Material material)
        {
            var calc = new Calculation
            {
                CalcName = input.CalcName,
                PrinterId = input.PrinterId,
                MaterialId = input.MaterialId,
                PartsProduced = input.PartsProduced,
                NumberOfBuilds = input.NumberOfBuilds,
                PartMass = input.PartMass!.Value,
                PartHeight = input.PartHeight!.Value,
                PartArea = input.PartArea!.Value,
                SupportMat = input.SupportMat
            };

            // Calculation logic
            calc.TotalMaterial = input.PartsProduced * input.PartMass.Value;
            calc.SupportMass = input.SupportMat * input.PartMass.Value;
            calc.TotalSupport = input.PartsProduced * calc.SupportMass;
            calc.TotalMaterialAllBuilds = input.NumberOfBuilds * printer.Machine_Build_Area * printer.Machine_Build_Height * material.Material_density / 1000;
            calc.Recycled = (calc.TotalMaterialAllBuilds - calc.TotalMaterial - calc.TotalSupport) * printer.Recycling_fraction;
            calc.Waste = calc.TotalMaterialAllBuilds - calc.Recycled - calc.TotalMaterial - calc.TotalSupport;
            calc.RequiredMaterial = calc.Waste + calc.TotalMaterial + calc.TotalSupport;
            calc.MaterialCost = calc.RequiredMaterial * material.Material_cost;

            calc.PrepTime = ((input.NumberOfBuilds - 1) * printer.Subsequent_build_preparation) + printer.First_time_build_preparation;
            calc.BuildPrepCost = calc.PrepTime * printer.FTE_salary_engineer;

            calc.PostTimePerPart = printer.Support_removal_time_labor_constant * (decimal)Math.Sqrt((double)input.PartArea);
            calc.TotalPostTime = calc.PostTimePerPart * input.PartsProduced;
            calc.PostProcessCost = calc.TotalPostTime * printer.FTE_salary_technician;

            calc.UpFront = printer.Purchase_cost * (1 + printer.Infrastructure_Cost);
            calc.AnnualDepreciation = printer.Cost_Of_Capital * calc.UpFront / (1 - (decimal)Math.Pow((double)(1 + printer.Cost_Of_Capital), -(double)printer.Machine_lifetime));
            calc.AnnualMaintenance = printer.Maintenance * printer.Purchase_cost;
            calc.AnnualMachineCost = calc.AnnualDepreciation + calc.AnnualMaintenance;
            calc.HoursPerYear = printer.Hours_per_day * printer.Days_per_week * 52 * printer.Machine_Uptime;
            calc.MachineCostPerHour = calc.AnnualMachineCost / calc.HoursPerYear;

            calc.WarmupTotal = input.NumberOfBuilds * printer.Time_per_machine_warm_up;
            calc.PartVolume = input.PartMass.Value * 1000 / material.Material_density;
            calc.PrintTime = input.PartsProduced * calc.PartVolume / printer.Machine_Build_Rate;
            calc.CooldownTotal = input.NumberOfBuilds * printer.Time_per_machine_cool_down;
            calc.ExchangeTime = input.NumberOfBuilds * (printer.Time_per_build_setup + printer.Time_per_build_removal);
            calc.MachineTime = calc.WarmupTotal + calc.PrintTime + calc.CooldownTotal + calc.ExchangeTime;
            calc.MachineCost = calc.MachineTime * calc.MachineCostPerHour;

            calc.OperatingTotalCost = printer.Additional_operating_cost * (calc.WarmupTotal + calc.PrintTime + calc.CooldownTotal);
            calc.Consumables = printer.Consumable_cost_per_build * input.NumberOfBuilds;
            calc.ConsumablesCost = calc.OperatingTotalCost + calc.Consumables;

            calc.LaborBuildTime = (calc.WarmupTotal + calc.PrintTime + calc.CooldownTotal) * printer.FTE_per_machine_supervised;
            calc.LaborExchangeTime = calc.ExchangeTime * printer.FTE_for_build_exchange;
            calc.LaborCost = (calc.LaborBuildTime + calc.LaborExchangeTime) * printer.FTE_salary_operator;

            calc.TotalCost = calc.MaterialCost + calc.BuildPrepCost + calc.PostProcessCost + calc.MachineCost + calc.ConsumablesCost + calc.LaborCost;

            return calc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calculation>>> GetCalculations()
        {
            return await _context.Calculations
                .Include(c => c.Printer)
                .Include(c => c.Material)
            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Calculation>> GetCalculation(int id)
        {
            try
            {
                var calculation = await _context.Calculations
                    .Include(c => c.Printer)
                    .Include(c => c.Material)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (calculation == null)
                    return NotFound();

                return calculation;
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging enabled
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }

        }
            [HttpPost("upload-photo/{id}")]
        public async Task<IActionResult> UploadPhoto(int id, IFormFile photo)
        {
            var calculation = await _context.Calculations.FindAsync(id);
            if (calculation == null)
                return NotFound();

            if (photo == null || photo.Length == 0)
                return BadRequest("No photo uploaded.");

            // Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Unsupported file type. Allowed types are: .jpg, .jpeg, .png, .webp");

            // Delete old photo if exists
            if (!string.IsNullOrWhiteSpace(calculation.ProductImage))
            {
                var oldPath = Path.Combine("wwwroot", calculation.ProductImage.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            // Save new photo
            var uploadsFolder = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Save file with unique name
            var uniqueFileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Store relative path in DB
            calculation.ProductImage = "/uploads/" + uniqueFileName;
            await _context.SaveChangesAsync();

            return Ok(new { calculation.ProductImage });
        }


        [HttpDelete("delete-photo/{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var calculation = await _context.Calculations.FindAsync(id);
            if (calculation == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(calculation.ProductImage))
            {
                var relativePath = calculation.ProductImage.TrimStart('/');
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                try
                {
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    // Optional: log this
                    return StatusCode(500, $"Error deleting photo: {ex.Message}");
                }
            }

            calculation.ProductImage = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalculation(int id)
        {
            var calculation = await _context.Calculations.FindAsync(id);
            if (calculation == null)
            {
                return NotFound();
            }

            _context.Calculations.Remove(calculation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("upload-stl")]
        public async Task<IActionResult> UploadStl(IFormFile stlFileInput, [FromForm] int materialId)
        {
             if (stlFileInput == null || stlFileInput.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                // Save to temp path
                var tempPath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(tempPath))
                {
                    await stlFileInput.CopyToAsync(stream);
                }

                // Create an instance of StlParser
                var parser = new StlParser();

                // Parse STL to DTO
                var stlDto = parser.ParseToDto(tempPath);

                // Delete temp file
                System.IO.File.Delete(tempPath);

                // Fetch material (replace this with your actual material retrieval logic)
                var material = await _context.Materials.FindAsync(materialId);
                if (material == null)
                    return BadRequest("Invalid material ID");
                
                    // Calculate PartArea (Length * Width)
                stlDto.PartArea = (decimal)(stlDto.Length * stlDto.Width);

                    // Calculate PartMass (Volume * density / 1000 for kg if density is g/cm3)
                stlDto.PartMass = (decimal)stlDto.Volume * material.Material_density / 1000m;
                
         
                return Ok(stlDto);
            }
            catch (Exception ex)
            {
                // Log ex here if you have logging
                return StatusCode(500, "Error parsing STL: " + ex.Message);
            }
        }

       [HttpPost("calculate")]
        public IActionResult Calculate(
            [FromForm] CalculationInputDto input, 
            IFormFile? stlFile,
            [FromForm] Printer printer,
            [FromForm] Material material)
        {
            StlInputDto? stlData = null;

            if (stlFile != null)
            {
                // Save and parse STL file
                var tempPath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(tempPath))
                {
                    stlFile.CopyTo(stream);
                }
                stlData = new StlParser().ParseToDto(tempPath);
                System.IO.File.Delete(tempPath);
            }

            if (stlData != null)
            {
                if (stlData.Height == null)
                {
                    return BadRequest("STL parsing failed to produce required values.");
                }
                input = CalculationMapper.MapStlToCalculationInput(stlData, input, material);
                input.PartHeight = (decimal)stlData.Height.Value;
                input.PartMass = stlData.PartMass.Value;
                input.PartArea = stlData.PartArea.Value;
            }
            else
            {
                if (input.PartHeight == null || input.PartMass == null || input.PartArea == null)
                {
                    return BadRequest("Missing required part dimensions when no STL file is uploaded.");
                }
            }

            var calculation = GenerateFullCalculation(input, printer, material);
            return Ok(calculation);
        }

}
}
