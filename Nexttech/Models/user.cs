using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexttech.Models
{
    public class User
{
    [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Pwd { get; set; } = "";
    public string E_mail { get; set; } = "";
    public string Role { get; set; } = "user"; // Default to "user"
}
}