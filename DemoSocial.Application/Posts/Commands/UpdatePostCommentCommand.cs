using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.Commands;

public sealed record UpdatePostCommentCommand(Guid PostId, Guid PostCommentId, string UpdatedText) : IRequest<OperationResult<PostComment>>;
