using Nexttech.Models;
using System.ComponentModel.DataAnnotations;
    public class Calculation
    {
        public int Id { get; set; }

        // Inputs
        public string CalcName { get; set; } = string.Empty;
        public int PrinterId { get; set; }
        public int MaterialId { get; set; }
        public int PartsProduced { get; set; }
        public int NumberOfBuilds { get; set; }
        public double PartMass { get; set; }
        public double PartHeight { get; set; }
        public double PartArea { get; set; }
        public double SupportMat { get; set; }

        // Outputs
        public double MaterialCost { get; set; }
        public double BuildPrepCost { get; set; }
        public double PostProcessCost { get; set; }
        public double MachineCost { get; set; }
        public double ConsumablesCost { get; set; }
        public double LaborCost { get; set; }
        public double TotalCost { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        
        [Required]
        public Printer Printer { get; set; }   // Reference to the Printer entity
        [Required]
        public Material Material { get; set; } // Reference to the Material entity
    }
