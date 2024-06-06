using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.Posts.Queries;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.QueryHandlers;

internal class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
{
    private readonly DataContext _context;
    public GetPostByIdQueryHandler(DataContext context) => _context = context;
    public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        OperationResult<Post> result = new();
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

        if (post is null)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.NotFound,
                Message = $"No post found with Id {request.PostId}"
            };
            result.Errors.Add(error);
            return result;
        }

        result.Payload = post;
        return result;
    }
}
