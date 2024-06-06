using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.UserProfiles.Queries;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly DataContext _context;
    
    public GetUserProfileByIdQueryHandler(DataContext context) => _context = context;

    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

        if (userProfile == null)
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

        result.Payload = userProfile;
        return result;
    }
}


