using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace DemoSocial.Application.Posts.CommandHandlers;

internal class UpdatePostCommentCommandHandler(DataContext context) : IRequestHandler<UpdatePostCommentCommand, OperationResult<PostComment>>
{
    private readonly DataContext _context = context;
    private OperationResult<PostComment> _result;
    private readonly PostErrorMessages _errorMessages = new();
    public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentCommand request, CancellationToken cancellationToken)
    {
		try
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);

			if (post is null) 
                _result.AddError(ErrorCode.NotFound, string.Format(_errorMessages.PostNotFound, request.PostId));
            
			var postComment = post?.Comments.FirstOrDefault(pc => pc.CommentId == request.PostCommentId);

			if (postComment is null) 
                _result.AddError(ErrorCode.NotFound, string.Format(_errorMessages.PostCommentNotFound, request.PostCommentId));

            postComment?.UpdatePostComment(request.UpdatedText);
			await _context.SaveChangesAsync();
			_result.Payload = postComment;
        }

        catch(PostCommentNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => 
                _result.AddError(ErrorCode.NotFound, $"{ex.Message}"));
        }
        catch (Exception ex)
        {
                _result.AddUnknownError($"{ex.Message}");
        }
        return _result;
    }
}
