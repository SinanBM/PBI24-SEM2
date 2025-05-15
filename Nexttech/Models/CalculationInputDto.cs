namespace Nexttech.Models
{
    public class CalculationInputDto
    {
        public string? CalcName { get; set; }
        public int PrinterId { get; set; }
        public int MaterialId { get; set; }
        public int PartsProduced { get; set; }
        public int NumberOfBuilds { get; set; }
        public decimal PartMass { get; set; }
        public decimal PartHeight { get; set; }
        public decimal PartArea { get; set; }
        public decimal SupportMat { get; set; }

    }
}