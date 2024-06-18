
using System.ComponentModel.DataAnnotations;

namespace DemoSocial.Api.Contracts.Post.Requests;

public record PostUpdate
{
    [Required]
    public String Text { get; set; }
}
