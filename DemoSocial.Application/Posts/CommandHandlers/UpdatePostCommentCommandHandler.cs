using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.Posts.Commands;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Posts.CommandHandlers;

internal class UpdatePostCommentCommandHandler(DataContext context) : IRequestHandler<UpdatePostCommentCommand, OperationResult<PostComment>>
{
    private readonly DataContext _context = context;
    public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentCommand request, CancellationToken cancellationToken)
    {
        OperationResult<PostComment> result = new();

		try
		{
			var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId);
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
			else
			{
				var postComment = post.Comments.FirstOrDefault(pc => pc.CommentId == request.PostCommentId);
                if (postComment is null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No post comment found with Id {request.PostCommentId}"
                    };
                    result.Errors.Add(error);
                    return result;
                }
                postComment.UpdatePostComment(request.UpdatedText);
                await _context.SaveChangesAsync();
                result.Payload = postComment;
            }
            //var postComment = await post.Comments.FirstOrDefaultAsync(pc => pc.CommentId == request.PostCommentId).FirstOrDefault();
            //if(postComment is null)
            //{
            //	result.IsError = true;
            //             var error = new Error
            //             {
            //                 Code = ErrorCode.NotFound,
            //                 Message = $"No post comment found with Id {request.PostCommentId}"
            //             };
            //             result.Errors.Add(error);
            //	return result;
            //}
            //postComment.UpdatePostComment(request.UpdatedText);
            //await _context.SaveChangesAsync();
            //result.Payload = postComment;

        }
		catch (Exception)
		{

			throw;
		}
		return result;
    }
}
