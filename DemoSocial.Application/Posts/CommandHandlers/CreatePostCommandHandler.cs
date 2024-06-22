using DemoSocial.Application.Abstractions;
using SharedKernel;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<CreatePostCommand, OperationResult<Post>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private OperationResult<Post> _result = new();
    public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var post = Post.CreatePost(
                request.UserProfileId, request.TextContent);
            _context.Posts.Add(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _result.Payload = post;
        }

        catch (PostNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(
                ErrorCode.ValidationError, $"{ex.Message}"));
        }

        catch (Exception ex)
        {
                _result.AddUnknownError($"{ex.Message}");
        }
        return _result;
    }
}
