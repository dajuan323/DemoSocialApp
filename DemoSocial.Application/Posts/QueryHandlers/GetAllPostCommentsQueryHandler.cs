using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.Posts.Queries;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.QueryHandlers;

internal class GetAllPostCommentsQueryHandler(DataContext context) : IRequestHandler<GetAllPostCommentsQuery, OperationResult<List<PostComment>>>
{
    private readonly DataContext _context = context;
    public async Task<OperationResult<List<PostComment>>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
    {
        OperationResult<List<PostComment>> result = new();
		try
		{
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId);

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

            result.Payload = post.Comments.ToList();
		}
        catch (Exception e)
        {
            Error error = new()
            {
                Code = ErrorCode.UnknownError,
                Message = $"{e.Message}"
            };
            result.IsError = true;
            result.Errors.Add(error);
        }

        return result;
    }
}
