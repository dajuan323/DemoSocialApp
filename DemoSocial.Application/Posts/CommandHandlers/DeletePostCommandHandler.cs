using DemoSocial.Application.Abstractions;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal class DeletePostCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<DeletePostCommand, OperationResult<Post>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private OperationResult<Post> _result = new();
    private readonly PostErrorMessages _errorMessages;
    public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        
        try
        {
            Post post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);
            if (post is null) _result.AddError(
                ErrorCode.NotFound, string.Format(_errorMessages.PostNotFound, request.PostId));
            _context.Posts.Remove(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _result.Payload = post;


        }
        catch (PostNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e =>
            {
                _result.AddError(ErrorCode.NotFound, $"{ex.Message}");
            });
        }
        catch (Exception ex)
        {
            _result.AddUnknownError($"{ex.Message}");
        }
        return _result;
    }
}
