namespace DemoSocial.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
        CreateMap<PostCommentCreate, AddCommentToPostCommand>();
        CreateMap<PostCommentUpdate, UpdatePostCommentCommand>();
    }
}
