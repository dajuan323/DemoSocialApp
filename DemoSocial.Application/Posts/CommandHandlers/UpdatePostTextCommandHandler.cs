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

internal class UpdatePostTextCommandHandler(DataContext context) : IRequestHandler<UpdatePostTextCommand, OperationResult<Post>>
{
    private readonly DataContext _context = context;
    public async Task<OperationResult<Post>> Handle(UpdatePostTextCommand request, CancellationToken cancellationToken)
    {
        OperationResult<Post> result = new();

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

            post.UpdatePostText(request.NewText);
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
