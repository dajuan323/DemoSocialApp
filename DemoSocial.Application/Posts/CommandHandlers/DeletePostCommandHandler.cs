using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.Posts.Commands;
using DemoSocial.Domain.Aggregates.PostAggregate;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
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

internal class DeletePostCommandHandler(DataContext context) : IRequestHandler<DeletePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context = context;
    public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        OperationResult<Post> result = new();
        Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

        try
		{
			
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

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);

            result.Payload = post;

            return result;

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
