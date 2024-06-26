﻿using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using SharedKernel;

namespace DemoSocial.Application.Posts.Commands;

public sealed record DeletePostCommand(Guid PostId) : IRequest<OperationResult<Post>>;

