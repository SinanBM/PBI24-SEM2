namespace Nexttech.Models
{
    public class CalculationInputDto
    {
        public string? CalcName { get; set; }
        public int PrinterId { get; set; }
        public int MaterialId { get; set; }
        public int PartsProduced { get; set; }
        public int NumberOfBuilds { get; set; }
        public double PartMass { get; set; }
        public double PartHeight { get; set; }
        public double PartArea { get; set; }
        public double SupportMat { get; set; }

    }
}