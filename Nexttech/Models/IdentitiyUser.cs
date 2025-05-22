using Microsoft.AspNetCore.Identity;

namespace Nexttech.Models;
public class NexttechUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}