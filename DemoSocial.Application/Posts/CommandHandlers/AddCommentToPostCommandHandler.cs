
using DemoSocial.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace DemoSocial.Application.Posts.CommandHandlers;

internal class AddCommentToPostCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<AddCommentToPostCommand, OperationResult<PostComment>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private OperationResult<PostComment> _result = new();
    private PostErrorMessages _errorMessages = new();

    public async Task<OperationResult<PostComment>> Handle(AddCommentToPostCommand request, CancellationToken cancellationToken)
    {
		try
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken: cancellationToken);
            if (post is null)
            {
                _result.AddError(ErrorCode.NotFound, string.Format(_errorMessages.PostNotFound, request.PostId));
            }

            var comment = PostComment.CreatePostComment(request.PostId, request.Text, request.UserProfileId);

            post.AddPostComment(comment);

            _context.Posts.Update(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _result.Payload = comment;
        }

        catch (PostCommentNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError,$"{ex.Message}"));
        }

		catch (Exception ex)
		{
                _result.AddUnknownError($"{ex.Message}");
        }

		return _result;
    }
}
