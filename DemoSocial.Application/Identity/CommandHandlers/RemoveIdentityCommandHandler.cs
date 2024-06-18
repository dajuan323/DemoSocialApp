using DemoSocial.Application.Identity;
using DemoSocial.Application.UserProfiles;
using Microsoft.EntityFrameworkCore;

namespace DemoSocial.Application.Identity.CommandHandlers;

internal sealed class RemoveIdentityCommandHandler(
    DataContext _context
) : IRequestHandler<RemoveIdentityCommand, OperationResult<bool>>
{
    OperationResult<bool> _result = new();
    IdentityErrorMessages _identityErrorMessages = new();
    UserProfileErrorMessages _profileErrorMessages = new();

    public async Task<OperationResult<bool>> Handle(RemoveIdentityCommand request, CancellationToken cancellationToken)
    {

        try
        {
            IdentityUser user = await _context.Users.FirstOrDefaultAsync(
            u => u.Id == request.IdentityUserId.ToString(), cancellationToken);

            if (user == null)
            {
                _result.AddError(ErrorCode.NotFound,
                    _identityErrorMessages.NonExistentIdentityUser);
                return _result;
            }

            UserProfile profile = await _context.UserProfiles.FirstOrDefaultAsync(
                up => up.IdentityId == request.IdentityUserId.ToString(), cancellationToken);
            if (profile == null)
            {
                _result.AddError(ErrorCode.NotFound,
                    _profileErrorMessages.UserProfileNotFound);
                return _result;
            }

            if (user.Id != request.RequestorGuid.ToString())
            {
                _result.AddError(ErrorCode.UnauthorizedAccountRemoval,
                    _identityErrorMessages.UnauthorizedAccountRemoval);
                return _result;
            }

            _context.UserProfiles.Remove(profile);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);

            _result.Payload = true;
        }
        catch (Exception ex)
        {
            _result.AddUnknownError($"{ex.Message}");
        }
        

        return _result;
    }
}
