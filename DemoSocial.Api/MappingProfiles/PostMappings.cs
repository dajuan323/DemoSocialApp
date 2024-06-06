using AutoMapper;
using DemoSocial.Api.Contracts.Post.Requests;
using DemoSocial.Api.Contracts.Post.Responses;
using DemoSocial.Application.Posts.Commands;
using DemoSocial.Domain.Aggregates.PostAggregate;

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
