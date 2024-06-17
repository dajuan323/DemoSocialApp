using SharedKernel;

namespace DemoSocial.Application.Posts.Commands;

public sealed record RemoveCommentCommand(
    Guid PostId,
    Guid CommentId,
    Guid UserProfileId) : IRequest<OperationResult<PostComment>>;

