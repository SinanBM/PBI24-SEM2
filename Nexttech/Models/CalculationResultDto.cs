namespace Nexttech.Models
{
    public class CalculationResultDto
    {
    public decimal MaterialCost { get; set; }
    public decimal BuildPrepCost { get; set; }
    public decimal PostProcessCost { get; set; }
    public decimal MachineCost { get; set; }
    public decimal Consumables { get; set; }
    public decimal Labor { get; set; }
    public decimal TotalCost { get; set; }
    }
}