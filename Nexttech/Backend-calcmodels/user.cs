using system.ComponentModel.DataAnnotations;

public class user
{

    [Required]
    [EmailAddress]
    public string Email {get; set;}

    [Required]
    public string Password {get; set;}
}