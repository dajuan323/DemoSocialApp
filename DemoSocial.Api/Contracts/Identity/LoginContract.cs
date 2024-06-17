namespace DemoSocial.Api.Contracts.Identity
{
    public record LoginContract
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]

        public string Password { get; set; }
    }
}
