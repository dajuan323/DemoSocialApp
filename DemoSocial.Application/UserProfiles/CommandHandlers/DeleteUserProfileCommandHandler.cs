using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.CommandHandlers;

internal class DeleteUserProfileCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserProfileCommad, OperationResult<UserProfile>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private OperationResult<UserProfile> _result = new(); 

    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommad request, CancellationToken cancellationToken)
    {
        UserProfile userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

        if (userProfile is null) 
        {
            _result.AddError(ErrorCode.NotFound, $"No User Profile found with Id {request.UserProfileId}");
            return _result;
        }
        _context.UserProfiles.Remove(userProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _result.Payload = userProfile;

        return _result;
    }
}
