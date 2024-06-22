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

internal class GetAllPostCommentsQueryHandler(IDataContext context) : IRequestHandler<GetAllPostCommentsQuery, OperationResult<List<PostComment>>>
{
    private readonly IDataContext _context = context;
    private OperationResult<List<PostComment>> _result = new();
    private readonly PostErrorMessages _errorMessages = new();
    public async Task<OperationResult<List<PostComment>>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
    {
        OperationResult<List<PostComment>> result = new();
		try
		{
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post is null)
                _result.AddError(ErrorCode.NotFound,  "Posts not found");

            result.Payload = post.Comments.ToList();
		}
        catch (Exception e)
        {
            _result.AddUnknownError($"{e.Message}");
        }

        return result;
    }
}
