using System.ComponentModel.DataAnnotations;

namespace Nexttech.Models;

public class UpdateUserDto
{
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

