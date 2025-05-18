namespace Nexttech.Models
{
    public class StlInputDto
    {
        public int TriangleCount { get; set; }
        public double Volume { get; set; }       // in cm³
        public float Length { get; set; }        // X
        public float Width { get; set; }         // Y
        public float? Height { get; set; }        // Z
        public bool IsWatertight { get; set; }
        public decimal? PartMass { get; set; }
        public decimal? PartArea { get; set; }

    }
}
