namespace DemoSocial.Api.Contracts.Post.Responses;

public record PostInteractionResponse
{
    public Guid InteractionId { get; set; }
    public string InteractionType { get; set; }
    public InteractionUser Author { get; set; }
}
