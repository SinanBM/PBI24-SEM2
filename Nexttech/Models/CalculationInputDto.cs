namespace Nexttech.Models
{
    public class CalculationInputDto
    {
        public string? CalcName { get; set; } //user provided
        public int PrinterId { get; set; } //user provided
        public int MaterialId { get; set; } //user provided
        public int PartsProduced { get; set; } //user provided
        public int NumberOfBuilds { get; set; } //user provided

        // Nullable fields: if null, use STL-derived values
        public decimal? PartMass { get; set; }    // kg //user or stl provided
        public decimal? PartHeight { get; set; }  // cm //user or stl provided
        public decimal? PartArea { get; set; }    // cm² //user or stl provided

        public decimal SupportMat { get; set; }   // user provided
    }
}
