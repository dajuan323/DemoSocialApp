using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.Commands;

public sealed record CreatePostCommand(
    Guid UserProfileId, string TextContent) : IRequest<OperationResult<Post>>;
