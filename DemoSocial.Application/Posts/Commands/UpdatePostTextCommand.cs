using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using SharedKernel;

namespace DemoSocial.Application.Posts.Commands;

public sealed record UpdatePostTextCommand(
    Guid PostId,
    string NewText,
    Guid UserProfileId) : IRequest<OperationResult<Post>>;