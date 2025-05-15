using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexttech.Models
{
    public class Material
{
    [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Material_cost { get; set; } //dollar/kg
    public decimal Material_density { get; set; } //g/cm^3
}
}