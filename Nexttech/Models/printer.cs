using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexttech.Models
{
    public class Printer
{
    // machine
    [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public decimal Purchase_cost { get; set; }
    public string Name { get; set; } = "";
    public decimal Machine_lifetime { get; set; }
    public decimal Cost_Of_Capital { get; set; }  //interest rate
    public decimal Infrastructure_Cost { get; set; } //as % of machine cost, up-front
    public decimal Maintenance { get; set; } //as % of machine cost, per year
    public decimal Machine_Build_Area { get; set; } //LxW
    public decimal Machine_Build_Height { get; set; } //H
    public decimal Machine_Build_Volume { get; set; }
    public decimal Machine_Build_Rate { get; set; }
    public decimal Machine_Uptime { get; set; }

    // process
    public int Packing_policy { get; set; } //2 = 2D, 3 = 3D
    public decimal Packing_fraction { get; set; } //planar or volumetric
    public decimal Recycling_fraction { get; set; } //% of unused material
    public decimal Additional_operating_cost { get; set; } //e.g., inert gas
    public decimal Consumable_cost_per_build { get; set; } //e.g., build plate
    public decimal First_time_build_preparation { get; set; } //engineer
    public decimal Subsequent_build_preparation { get; set; } //engineer
    public decimal Time_per_build_setup { get; set; } //operator, machine
    public decimal Time_per_build_removal { get; set; } //operator, machine
    public decimal Time_per_machine_warm_up { get; set; } //machine
    public decimal Time_per_machine_cool_down { get; set; } //machine

    // post process
    public decimal Support_removal_time_labor_constant { get; set; } // t=c*A^0.5 [hr,cm^2]

    // operations
    public decimal Hours_per_day { get; set; }
    public decimal Days_per_week { get; set; }
    public decimal FTE_per_machine_supervised { get; set; } //during build
    public decimal FTE_for_build_exchange { get; set; }
    public decimal FTE_for_support_removal { get; set; }
    public decimal FTE_salary_engineer { get; set; }
    public decimal FTE_salary_operator { get; set; }
    public decimal FTE_salary_technician { get; set; }
}
}