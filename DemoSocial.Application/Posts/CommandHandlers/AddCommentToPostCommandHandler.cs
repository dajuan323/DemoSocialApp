
using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace DemoSocial.Application.Posts.CommandHandlers;

internal class AddCommentToPostCommandHandler : IRequestHandler<AddCommentToPostCommand, OperationResult<PostComment>>
{
    private readonly DataContext _context;
    private OperationResult<PostComment> _result = new();
    private PostErrorMessages _errorMessages = new();

    public AddCommentToPostCommandHandler(DataContext context)
    {
        _context = context;
    }

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
            await _context.SaveChangesAsync();
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
