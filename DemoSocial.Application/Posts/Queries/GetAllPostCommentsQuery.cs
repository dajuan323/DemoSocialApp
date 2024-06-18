using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using SharedKernel;


namespace DemoSocial.Application.Posts.Queries;

public sealed record GetAllPostCommentsQuery(Guid PostId) : IRequest<OperationResult<List<PostComment>>>;

