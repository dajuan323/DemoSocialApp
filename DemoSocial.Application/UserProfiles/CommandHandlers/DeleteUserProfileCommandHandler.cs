using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.CommandHandlers;

internal class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommad, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public DeleteUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommad request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        UserProfile? userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

        if (userProfile is null)
        {
            result.IsError = true;
            var error = new Error
            {
                Code = ErrorCode.NotFound,
                Message = $"No User Profile found with Id {request.UserProfileId}"
            };
            result.Errors.Add(error);
            return result;
        }

        _context.UserProfiles.Remove(userProfile);
        await _context.SaveChangesAsync(cancellationToken);

        result.Payload = userProfile;

        return result;
    }
}
