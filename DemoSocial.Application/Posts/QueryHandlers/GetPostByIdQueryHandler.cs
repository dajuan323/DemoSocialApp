using DemoSocial.Application.Posts.Queries;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.QueryHandlers;

internal class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
{
    private readonly DataContext _context;
    private OperationResult<Post> _result = new();
    private readonly PostErrorMessages _errorMessages = new();
    public GetPostByIdQueryHandler(DataContext context) => _context = context;
    public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post is null)
            {
                _result.AddError(
                ErrorCode.NotFound, string.Format(
                    _errorMessages.PostNotFound, request.PostId));
                return _result;
            }
            _result.Payload = post;
        }
        catch (Exception e)
        {
            _result.AddUnknownError($"{e.Message}");
        }
        return _result;
    }
}
