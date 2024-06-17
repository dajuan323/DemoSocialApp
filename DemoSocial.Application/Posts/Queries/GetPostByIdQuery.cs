using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.Queries;

public record GetPostByIdQuery(Guid PostId) : IRequest<OperationResult<Post>>;

