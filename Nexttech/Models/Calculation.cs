using Nexttech.Models;
using System.ComponentModel.DataAnnotations;
    public class Calculation
    {
    public int Id { get; set; }
    public string? CalcName { get; set; }
    public int PrinterId { get; set; }
    public int MaterialId { get; set; }
    public int PartsProduced { get; set; }
    public int NumberOfBuilds { get; set; }
    public decimal PartMass { get; set; }
    public decimal PartHeight { get; set; }
    public decimal PartArea { get; set; }
    public decimal SupportMat { get; set; }

    public decimal TotalMaterial { get; set; }
    public decimal SupportMass { get; set; }
    public decimal TotalSupport { get; set; }
    public decimal TotalMaterialAllBuilds { get; set; }
    public decimal Recycled { get; set; }
    public decimal Waste { get; set; }
    public decimal RequiredMaterial { get; set; }
    public decimal MaterialCost { get; set; }

    public decimal PrepTime { get; set; }
    public decimal BuildPrepCost { get; set; }

    public decimal PostTimePerPart { get; set; }
    public decimal TotalPostTime { get; set; }
    public decimal PostProcessCost { get; set; }

    public decimal UpFront { get; set; }
    public decimal AnnualDepreciation { get; set; }
    public decimal AnnualMaintenance { get; set; }
    public decimal AnnualMachineCost { get; set; }
    public decimal HoursPerYear { get; set; }
    public decimal MachineCostPerHour { get; set; }

    public decimal WarmupTotal { get; set; }
    public decimal PartVolume { get; set; }
    public decimal PrintTime { get; set; }
    public decimal CooldownTotal { get; set; }
    public decimal ExchangeTime { get; set; }
    public decimal MachineTime { get; set; }
    public decimal MachineCost { get; set; }

    public decimal OperatingTotalCost { get; set; }
    public decimal Consumables { get; set; }
    public decimal ConsumablesCost { get; set; }

    public decimal LaborBuildTime { get; set; }
    public decimal LaborExchangeTime { get; set; }
    public decimal LaborCost { get; set; }

    public decimal TotalCost { get; set; }

    public DateTime CreatedAt { get; set; }

    public  Printer Printer { get; set; }
    public Material Material { get; set; }
    public string? ProductImage { get; set; }

    }