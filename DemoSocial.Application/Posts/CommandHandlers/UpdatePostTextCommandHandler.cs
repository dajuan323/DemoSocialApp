using DemoSocial.Application.Posts.Commands;
using DemoSocial.Domain.Aggregates.PostAggregate;
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

internal class UpdatePostTextCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdatePostTextCommand, OperationResult<Post>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private OperationResult<Post> _result = new();
    private readonly PostErrorMessages _errorMessages;
    public async Task<OperationResult<Post>> Handle(UpdatePostTextCommand request, CancellationToken cancellationToken)
    {
		try
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

            if (post is null)
            {
                _result.AddError(ErrorCode.NotFound, string.Format(
                    _errorMessages.PostNotFound, request.PostId));
                return _result;
            }
                

            if (post.UserProfileId != request.UserProfileId)
            {
                _result.AddError(ErrorCode.PostUpdateNotPossible, _errorMessages.PostUpdateNotPossible);
                return _result;
            }
                
            

            post?.UpdatePostText(request.NewText);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _result.Payload = post; 
        }

        catch (PostNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError, e));
        }

        catch (Exception ex)
        {
            _result.AddUnknownError($"{ex.Message}");
        }
        return _result;
    }
}
