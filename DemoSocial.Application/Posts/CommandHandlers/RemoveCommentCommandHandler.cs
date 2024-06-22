using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Runtime.CompilerServices;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal sealed class RemoveCommentCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveCommentCommand, OperationResult<PostComment>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly OperationResult<PostComment> _result = new();
    PostErrorMessages _errorMessages = new();
    public async Task<OperationResult<PostComment>> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.PostId == request.PostId);
        if (post == null)
        {
            _result.AddError(ErrorCode.NotFound, 
                _errorMessages.PostNotFound);
            return _result;
        }
        var comment = post.Comments
            .FirstOrDefault(c => c.CommentId == request.CommentId);

        if (comment == null)
        {
            _result.AddError(ErrorCode.NotFound,
                _errorMessages.PostCommentNotFound);
            return _result;
        }

        if (comment.UserProfileId != request.UserProfileId)
        {
            _result.AddError(ErrorCode.CommentRemovalNotAuthorized,
                _errorMessages.CommentUpdateNotAuthorized);
            return _result;
        }

        post.RemoveComment(comment);
        _context.Posts.Update(post);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _result.Payload = comment;
        return _result;
    }
}
