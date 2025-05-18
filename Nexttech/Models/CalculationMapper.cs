using Nexttech.Models;

namespace Nexttech.Services
{
    public static class CalculationMapper
    {
        public static CalculationInputDto MapStlToCalculationInput(StlInputDto stl, CalculationInputDto input, Material material)
        {
            if (input.PartHeight == null)
                input.PartHeight = (decimal)stl.Height;

            if (input.PartArea == null)
                input.PartArea = (decimal)(stl.Length * stl.Width);

            if (input.PartMass == null && material != null)
                input.PartMass = (decimal)stl.Volume * material.Material_density / 1000m;

            return input;
        }
    }
}
