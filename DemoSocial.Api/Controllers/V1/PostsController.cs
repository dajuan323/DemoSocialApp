using Asp.Versioning;
using AutoMapper;
using DemoSocial.Api.Contracts.Common;
using DemoSocial.Api.Contracts.Post.Requests;
using DemoSocial.Api.Contracts.Post.Responses;
using DemoSocial.Api.Filters;
using DemoSocial.Application.Posts.Commands;
using DemoSocial.Application.Posts.Queries;
using DemoSocial.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemoSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class PostsController(IMediator mediator, IMapper mapper) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;


    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var result = await _mediator.Send(new GetAllPostsQuery());
        var mapped = _mapper.Map<List<PostResponse>>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);

        
    }

    [HttpGet]
    [Route(template:ApiRoutes.Posts.IdRoute)]
    [ValidateGuid(key:"id")]
    public async Task<IActionResult> GetById(string id)
    {
        var postId = Guid.Parse(id);
        var query = new GetPostByIdQuery(postId);
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
    [ValidateGuid(key:"id")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePost([FromBody] PostUpdate updatedPost, string id)
    {
        var command = new UpdatePostTextCommand
        (
            PostId : Guid.Parse(id),
            NewText : updatedPost.Text
        );
        var result = await _mediator.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(template:ApiRoutes.Posts.IdRoute)]
    [ValidateGuid(key:"id")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var command = new DeletePostCommand(Guid.Parse(id));
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(template:ApiRoutes.Posts.PostComments)]
    [ValidateGuid(key:"postId")]
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
    [ValidateGuid(key:"postId")]
    [ValidateModel]
    public async Task<IActionResult> AddcommentToPost(string postId, [FromBody] PostCommentCreate  postComment)
    {
        var IsValidGuid = Guid.TryParse(postComment.UserProfileId, out var userProfileId);
        if (!IsValidGuid)
        {
                var apiError = new ErrorResponse();


                apiError.StatusCode = 404;
                apiError.StatusPhrase = "Bad Request";
                apiError.Timestamp = DateTime.Now;
                apiError.Errors.Add("Provided user profile id is not in valid GUID format.");

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
    [ValidateGuid(key:"postId")]
    [ValidateGuid(key:"commentId")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePostComment(string postId, string commentId, [FromBody] PostCommentUpdate updatedComment)
    {
        var command = new UpdatePostCommentCommand
        (
            PostId : Guid.Parse(postId),
            PostCommentId: Guid.Parse(commentId),
            UpdatedText: updatedComment.Text
        );
        var result = await _mediator.Send(command);

        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }
}
