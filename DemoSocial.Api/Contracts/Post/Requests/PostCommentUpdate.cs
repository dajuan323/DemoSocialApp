using System.ComponentModel.DataAnnotations;

namespace DemoSocial.Api.Contracts.Post.Requests;

public record PostCommentUpdate
{
    [Required]
    public string Text { get; set; }
}
