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
    public double PartMass { get; set; }
    public double PartHeight { get; set; }
    public double PartArea { get; set; }
    public double SupportMat { get; set; }

    public double TotalMaterial { get; set; }
    public double SupportMass { get; set; }
    public double TotalSupport { get; set; }
    public double TotalMaterialAllBuilds { get; set; }
    public double Recycled { get; set; }
    public double Waste { get; set; }
    public double RequiredMaterial { get; set; }
    public double MaterialCost { get; set; }

    public double PrepTime { get; set; }
    public double BuildPrepCost { get; set; }

    public double PostTimePerPart { get; set; }
    public double TotalPostTime { get; set; }
    public double PostProcessCost { get; set; }

    public double UpFront { get; set; }
    public double AnnualDepreciation { get; set; }
    public double AnnualMaintenance { get; set; }
    public double AnnualMachineCost { get; set; }
    public double HoursPerYear { get; set; }
    public double MachineCostPerHour { get; set; }

    public double WarmupTotal { get; set; }
    public double PartVolume { get; set; }
    public double PrintTime { get; set; }
    public double CooldownTotal { get; set; }
    public double ExchangeTime { get; set; }
    public double MachineTime { get; set; }
    public double MachineCost { get; set; }

    public double OperatingTotalCost { get; set; }
    public double Consumables { get; set; }
    public double ConsumablesCost { get; set; }

    public double LaborBuildTime { get; set; }
    public double LaborExchangeTime { get; set; }
    public double LaborCost { get; set; }

    public double TotalCost { get; set; }

    public DateTime CreatedAt { get; set; }

    public Printer Printer { get; set; }
    public Material Material { get; set; }
    public string? ProductImage { get; set; }

    }