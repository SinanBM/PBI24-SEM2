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
    public double Purchase_cost { get; set; }
    public string Name { get; set; } = "";
    public double Machine_lifetime { get; set; }
    public double Cost_Of_Capital { get; set; }  //interest rate
    public double Infrastructure_Cost { get; set; } //as % of machine cost, up-front
    public double Maintenance { get; set; } //as % of machine cost, per year
    public double Machine_Build_Area { get; set; } //LxW
    public double Machine_Build_Height { get; set; } //H
    public double Machine_Build_Volume { get; set; }
    public double Machine_Build_Rate { get; set; }
    public double Machine_Uptime { get; set; }

    // process
    public int Packing_policy { get; set; } //2 = 2D, 3 = 3D
    public double Packing_fraction { get; set; } //planar or volumetric
    public double Recycling_fraction { get; set; } //% of unused material
    public double Additional_operating_cost { get; set; } //e.g., inert gas
    public double Consumable_cost_per_build { get; set; } //e.g., build plate
    public double First_time_build_preparation { get; set; } //engineer
    public double Subsequent_build_preparation { get; set; } //engineer
    public double Time_per_build_setup { get; set; } //operator, machine
    public double Time_per_build_removal { get; set; } //operator, machine
    public double Time_per_machine_warm_up { get; set; } //machine
    public double Time_per_machine_cool_down { get; set; } //machine

    // post process
    public double Support_removal_time_labor_constant { get; set; } // t=c*A^0.5 [hr,cm^2]

    // operations
    public double Hours_per_day { get; set; }
    public double Days_per_week { get; set; }
    public double FTE_per_machine_supervised { get; set; } //during build
    public double FTE_for_build_exchange { get; set; }
    public double FTE_for_support_removal { get; set; }
    public double FTE_salary_engineer { get; set; }
    public double FTE_salary_operator { get; set; }
    public double FTE_salary_technician { get; set; }
}
}