namespace DemoSocial.Api.Contracts.Identity;

public record UserRegistrationContract
{
    [Required]
    [EmailAddress]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(70)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(70)]
    public string LastName { get; set; }

    public string  Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}
