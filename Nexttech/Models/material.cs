namespace Nexttech.Models
{
    public class Material
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public double material_cost { get; set; } //dollar/kg
    public double material_density { get; set; } //g/cm^3
}
}