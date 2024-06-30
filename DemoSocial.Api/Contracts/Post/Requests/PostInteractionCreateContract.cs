namespace DemoSocial.Api.Contracts.Post.Requests;

public record PostInteractionCreateContract
{
    [Required]
    public InteractionType InteractionType { get; set; }
}
