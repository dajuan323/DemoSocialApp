using DemoSocial.Api.Extensions;
using SharedKernel;

namespace DemoSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize( AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PostsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PostsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    // Post

    [HttpGet]
    //[Authorize]
    public async Task<IActionResult> GetAllPosts()
    {
        var result = await _mediator.Send(new GetAllPostsQuery());
        var mapped = _mapper.Map<List<PostResponse>>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);

        
    }

    [HttpGet]
    [Route(template:ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetById(string postId)
    {
        var parsedPostId = Guid.Parse(postId);
        var query = new GetPostByIdQuery(parsedPostId);
        var result = await _mediator.Send(query);
        var mapped = _mapper.Map<PostResponse>(result.Payload);

        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
    }
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreatePost([FromBody] PostCreate newPost)
    {
        var command = new CreatePostCommand( Guid.Parse(newPost.UserProfileId), newPost.TextContent);
        var result = await _mediator.Send(command);
        var mapped = _mapper.Map<PostResponse>(result.Payload);

        return result.IsError ? HandleErrorResponse(result.Errors)  
            : CreatedAtAction(nameof(GetById), new { id = result.Payload.UserProfileId}, mapped); 


    }

    [HttpPatch]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePost([FromBody] PostUpdate updatedPost, string postId, string profileId)
    {
        var command = new UpdatePostTextCommand
        (
            PostId : Guid.Parse(postId),
            NewText : updatedPost.Text,
            UserProfileId : Guid.Parse(profileId)
        );
        var result = await _mediator.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(template:ApiRoutes.Posts.IdRoute)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> DeletePost(string postId)
    {
        var command = new DeletePostCommand(Guid.Parse(postId));
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }


    // Post Comments

    [HttpGet]
    [Route(template:ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetPostComments(string postId)
    {
        var query = new GetAllPostCommentsQuery(Guid.Parse(postId));
        var result = await _mediator.Send(query);

        if (result.IsError)  HandleErrorResponse(result.Errors);

        var comments = _mapper.Map<List<PostCommentResponse>>(result.Payload);
        return Ok(comments);
    }

    [HttpPost]
    [Route(template:ApiRoutes.Posts.PostComments)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> AddcommentToPost(string postId, [FromBody] PostCommentCreate  postComment)
    {
        var IsValidGuid = Guid.TryParse(postComment.UserProfileId, out var userProfileId);
        if (!IsValidGuid)
        {
            ErrorResponse apiError = new()
            {
                StatusCode = 404,
                StatusPhrase = "Bad Request",
                Timestamp = DateTime.Now,
                Errors = ["Provided user profile id is not in valid GUID format."]
            };

            return NotFound(apiError);            
        }

        var command = new AddCommentToPostCommand
            (
                Guid.Parse(postId),
                userProfileId,
                postComment.Text
            );

        var result = await _mediator.Send(command);
        if (result.IsError) return HandleErrorResponse(result.Errors);

        var newComment = _mapper.Map<PostCommentResponse>(result.Payload);
        return Ok(newComment);

        
    }

    [HttpPatch]
    [Route(template:ApiRoutes.Posts.CommentById)]
    [ValidateGuid("postId")]
    [ValidateGuid("commentId")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePostComment(string postId, string commentId, [FromBody] PostCommentUpdate updatedComment)
    {
        UpdatePostCommentCommand command = new UpdatePostCommentCommand
        (
            PostId : Guid.Parse(postId),
            PostCommentId: Guid.Parse(commentId),
            UpdatedText: updatedComment.Text
        );
        OperationResult<PostComment> result = await _mediator.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidateGuid("postId")]
    public async Task<IActionResult> GetPostInteractions(string postId, CancellationToken cancellation)
    {
        Guid postGuid = Guid.Parse(postId);
        GetPostInteractionsQuery query = new(PostId: postGuid);
        OperationResult<List<PostInteraction>> result = await _mediator.Send(query, cancellation);

        return result.IsError ? HandleErrorResponse(result.Errors)
            : Ok(_mapper.Map<List<PostInteractionResponse>>(result.Payload));

    }


    // Post Interactions

    [HttpPost]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidateGuid("postId")]
    [ValidateModel]
    public async Task<IActionResult> AddPostInteraction(string postId, PostInteractionCreateContract interaction,
            CancellationToken _)
    {
        Guid postGuid = Guid.Parse(postId);
        Guid userProfileId = HttpContext.GetUserProfileIdByClaimValue();
        AddInteraction command = new(
            PostId: postGuid,
            UserProfileId: userProfileId,
            InteractionType: interaction.InteractionType);

        OperationResult<PostInteraction> result = await _mediator.Send(command, _);

        return result.IsError ? HandleErrorResponse(result.Errors)
            : Ok(_mapper.Map<PostInteractionResponse>(result.Payload));
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.InteractionById)]
    [ValidateGuid("postId", "interactionId")]
    public async Task<IActionResult> RemoveInteraction(string postId, string interactionId,
        CancellationToken token)
    {
        Guid postGuid = Guid.Parse(postId);
        Guid interaactionGuid = Guid.Parse(interactionId);
        Guid userProfileGuid = HttpContext.GetUserProfileIdByClaimValue();
        DeleteInteractionCommand command = new(
            PostId: postGuid, 
            InteractionId: interaactionGuid,
            UserProfileId: userProfileGuid);
        
        OperationResult<PostInteraction> result = await _mediator.Send(command, token);
        
        return result.IsError ? HandleErrorResponse(result.Errors) : 
            Ok(_mapper.Map<PostInteractionResponse>(result.Payload));
    }

    //[HttpDelete]
    //[Route(template: ApiRoutes.Posts.CommentById)]
    //[ValidateGuid("postId")]
    //[ValidateGuid("commentId")]
    //public async Task<IActionResult> DeletePostComment(string postId, string commentId)
    //{
    //     var userProfileId = HttpContext.getu

    //}
}
