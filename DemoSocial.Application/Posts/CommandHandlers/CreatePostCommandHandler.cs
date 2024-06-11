namespace DemoSocial.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler(DataContext context) : IRequestHandler<CreatePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context = context;
    public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        OperationResult<Post> result = new();


        try
        {
            var post = Post.CreatePost(request.UserProfileId, request.TextContent);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            result.Payload = post;
        }

        catch (PostNotValidException ex)
        {
            result.IsError = true;
            ex.ValidationErrors.ForEach(e =>
            {
                Error error = new()
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{ex.Message}"
                };
                result.Errors.Add(error);
            });
        }

        catch (Exception e)
        {
            Error error = new()
            {
                Code = ErrorCode.UnknownError,
                Message = $"{e.Message}"
            };
            result.IsError = true;
            result.Errors.Add(error);
        }
        return result;
    }
}
