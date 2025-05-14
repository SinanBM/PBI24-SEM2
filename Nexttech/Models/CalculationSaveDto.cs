namespace Nexttech.Models
{
    public class CalculationSaveDto : CalculationInputDto
    {
        public int Id { get; set; }
        public double MaterialCost { get; set; }
        public double PrepCost { get; set; }
        public double PostCost { get; set; }
        public double MachineCost { get; set; }
        public double Consumables { get; set; }
        public double Labor { get; set; }
        public double TotalCost { get; set; }
    }
}