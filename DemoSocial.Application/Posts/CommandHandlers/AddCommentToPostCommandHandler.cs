using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.Posts.Commands;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Domain.Exceptions;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal class AddCommentToPostCommandHandler(DataContext context) : IRequestHandler<AddCommentToPostCommand, OperationResult<PostComment>>
{
    private readonly DataContext _context = context;
    public async Task<OperationResult<PostComment>> Handle(AddCommentToPostCommand request, CancellationToken cancellationToken)
    {
        OperationResult<PostComment> result = new();

		try
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);
            if (post is null)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = $"No post found with Id {request.PostId}"
                };
                result.Errors.Add(error);
                return result;
            }

            var comment = PostComment.CreatePostComment(request.PostId, request.Text, request.UserProfileId);

            post.AddPostComment(comment);

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            result.Payload = comment;
        }

        catch(PostCommentNotValidException ex)
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

		catch (Exception ex)
		{

			var error = new Error { Code = ErrorCode.UnknownError, 
				Message = $"{ex.Message}"};
			result.IsError = true;
			result.Errors.Add(error);
		}

		return result;
    }
}
