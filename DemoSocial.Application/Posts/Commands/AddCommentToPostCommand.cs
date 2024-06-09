using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;


namespace DemoSocial.Application.Posts.Commands;

public sealed record AddCommentToPostCommand(
   Guid PostId, Guid UserProfileId, string Text) : IRequest<OperationResult<PostComment>>;
