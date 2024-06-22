using DemoSocial.Application.UserProfiles.Queries;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, OperationResult<UserProfile>>
{
    private readonly IDataContext _context;
    private OperationResult<UserProfile> _result = new();
    private readonly UserProfileErrorMessages _errorMessages = new();
    
    
    public GetUserProfileByIdQueryHandler(IDataContext context) => _context = context;

    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(
                up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);
            if (userProfile == null)
            {
                _result.AddError(ErrorCode.NotFound, string.Format(
                    _errorMessages.UserProfileNotFound, request.UserProfileId));
                return _result;
            }
            _result.Payload = userProfile;
        }
        catch (Exception ex)
        {
            _result.AddUnknownError($"{ex.Message}");
        }
        return _result;
    }
}


