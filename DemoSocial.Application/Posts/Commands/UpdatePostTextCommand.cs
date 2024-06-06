using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace DemoSocial.Application.Posts.Commands;

public sealed record UpdatePostTextCommand(Guid PostId, string NewText) : IRequest<OperationResult<Post>>;