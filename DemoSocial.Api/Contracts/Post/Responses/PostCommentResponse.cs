namespace DemoSocial.Api.Contracts.Post.Responses;

public record PostCommentResponse
{
    public string PostId { get; set; }
    public string CommentId { get; set; }
    public string UserProfileId { get; set; }
    public string Text { get; set; }
   
}
