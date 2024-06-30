namespace DemoSocial.Api;

public class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public class UserProfiles
    {
        public const string Idroute = "{id}";
    }

    public class Posts
    {
        public const string IdRoute = "{postId}";
        public const string PostComments = "{postId}/comments";
        public const string CommentById = "{postId}/comments/{commentId}";
        public const string AddInteraction = "{postId}/interactions";
        public const string InteractionById = "{postId}/interactions/{interactionId}";
        public const string PostInteractions = "{postId}/interactions";
    }

    public class Identiy
    {
        public const string Login = "login";
        public const string Registration = "registration";
        public const string IdentityById = "{identityUserId}";
    }
}


