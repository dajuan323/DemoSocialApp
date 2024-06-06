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

internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, OperationResult<List<Post>>>
{
    private readonly DataContext _context;
    public GetAllPostsQueryHandler(DataContext context) => _context = context;
    public async Task<OperationResult<List<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        OperationResult<List<Post>> result = new();

        try
        {
            result.Payload = await _context.Posts.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
		{

            Error error = new()
            {
                Code = ErrorCode.UnknownError,
                Message = $"{ex.Message}"
            };
            result.IsError = true;
            result.Errors.Add(error);
            
		}

        return result;



    }
}
