namespace DemoSocial.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
        CreateMap<PostCommentCreate, AddCommentToPostCommand>();
        CreateMap<PostCommentUpdate, UpdatePostCommentCommand>();
        CreateMap<PostInteraction, PostInteractionResponse>()
            .ForMember(dest
                => dest.InteractionType, opt
                => opt.MapFrom(src
                => src.InteractionType.ToString()))
            .ForMember(dest => dest.Author , opt
                => opt.MapFrom(src => src.UserProfile));
            
    }
}
