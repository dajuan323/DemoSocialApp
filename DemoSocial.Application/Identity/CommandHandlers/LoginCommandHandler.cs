using DemoSocial.Application.Abstractions;
using DemoSocial.Application.Services;
using Microsoft.EntityFrameworkCore;


namespace DemoSocial.Application.Identity.CommandHandlers;

internal class LoginCommandHandler(
    IDataContext _context,
    IdentityService _identityService,
    UserManager<IdentityUser> _userManager) : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private OperationResult<string> _result = new();
    private readonly IdentityErrorMessages _errorMessages = new();

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            IdentityUser identityUser = await ValidateAndGetIdentityAsync(request);
            if (_result.IsError) return _result;

            UserProfile userProfile = await GetUserProfile(identityUser, cancellationToken);
            if (_result.IsError) return _result;

            _result.Payload = GetJwtString(identityUser, userProfile);
        }
        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(
                ErrorCode.ValidationError, $"{ex.Message}"));
        }
        catch (Exception e)
        {
            _result.AddError(ErrorCode.NotFound, $"{e.Message}");
        }
        return _result;
    }

    #region private methods

    private async Task<IdentityUser> ValidateAndGetIdentityAsync(LoginCommand request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Username);

        // Remember, this is REVERSE of registration.
        if (identityUser is null)
        {
            _result.AddError(ErrorCode.NotFound, _errorMessages.NonExistentIdentityUser);
        }

        bool validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!validPassword) _result.AddError(ErrorCode.IncorrectPassword, _errorMessages.IncorrectPassword);

        return identityUser;
    }

    private string GetJwtString(IdentityUser identityUser, UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
         {
            new Claim(type: JwtRegisteredClaimNames.Sub, value: identityUser.Email),
            new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
            new Claim(type: JwtRegisteredClaimNames.Email, value: identityUser.Email),
            new Claim(type: "IdentityId", value: identityUser.Id),
            new Claim(type: "UserProfileId", value: userProfile.UserProfileId.ToString())
         });

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        if (token is null) _result.AddError(ErrorCode.IdentityCreationFailed, _errorMessages.CreateTokenFailure);

        return _identityService.WriteToken(token);
    }

    private async Task<UserProfile> GetUserProfile(IdentityUser identityUser, CancellationToken cancellationToken)
    {
        UserProfile userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(predicate: up => up.IdentityId == identityUser.Id, cancellationToken: cancellationToken);

        if (userProfile is null) _result.AddError(ErrorCode.NotFound, _errorMessages.NonExistentIdentityUser);
        return userProfile;
    }

    #endregion end
}
