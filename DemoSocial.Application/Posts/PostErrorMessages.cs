using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts;

public record PostErrorMessages
{
    public string PostCommentNotFound = "No post comment found with Id {0}";

    public string PostsNotAvaialble = "No posts available.";

    public string PostNotFound = $"No post found with Id {0}";

    public string PostUpdateNotPossible = "Post update not possible.  Invalid post owner.";

    public string CommentUpdateNotAuthorized = "Comment update not authorized.";
}
