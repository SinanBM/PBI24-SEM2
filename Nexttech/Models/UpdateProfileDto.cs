using System.ComponentModel.DataAnnotations;

namespace Nexttech.Models
{
    public class UpdateProfileDto
    {
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
    }
}