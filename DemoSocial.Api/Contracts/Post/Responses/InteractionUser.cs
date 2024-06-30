namespace DemoSocial.Api.Contracts.Post.Responses;

public record InteractionUser
{
    public Guid UserProfileId { get; set; }
    public string FullName { get; set; }
    public string City { get; set; }
}
