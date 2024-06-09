using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;


namespace DemoSocial.Application.Posts.Queries;

public sealed record GetAllPostCommentsQuery(Guid PostId) : IRequest<OperationResult<List<PostComment>>>;

