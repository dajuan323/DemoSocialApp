using DemoSocial.Application.Posts.Queries;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, OperationResult<List<Post>>>
{
    private readonly IDataContext _context;
    private OperationResult<List<Post>> _result = new();
    private readonly PostErrorMessages _errorMessages;
    public GetAllPostsQueryHandler(IDataContext context) => _context = context;
    public async Task<OperationResult<List<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var posts = await _context.Posts.ToListAsync(cancellationToken);
            if (posts == null)
            {
                _result.AddError(ErrorCode.NotFound, _errorMessages.PostsNotAvaialble);
                return _result;
            }
                
            _result.Payload =  posts;
        }
        
        catch (Exception ex)
		{
            _result.AddUnknownError($"{ex.Message}");
		}
        return _result;
    }
}
