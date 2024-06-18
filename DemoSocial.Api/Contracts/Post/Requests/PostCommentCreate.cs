namespace DemoSocial.Api.Contracts.Post.Requests;

public record PostCommentCreate
{
    [Required]
    public string UserProfileId { get; set; }
    [Required]
    public string Text { get; set; }
}
