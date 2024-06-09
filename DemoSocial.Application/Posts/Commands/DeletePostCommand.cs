using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;

namespace DemoSocial.Application.Posts.Commands;

public sealed record DeletePostCommand(Guid PostId) : IRequest<OperationResult<Post>>;

