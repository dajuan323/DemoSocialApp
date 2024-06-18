using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using SharedKernel;


namespace DemoSocial.Application.Posts.Commands;

public sealed record AddCommentToPostCommand(
   Guid PostId, Guid UserProfileId, string Text) : IRequest<OperationResult<PostComment>>;
