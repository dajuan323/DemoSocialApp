using DemoSocial.Application.Services;
using Microsoft.EntityFrameworkCore.Storage;


namespace DemoSocial.Application.Identity.CommandHandlers;

internal class RegisterIdentityUserCommandHandler(
    IDataContext _context,
    IUnitOfWork _unitOfWork,
    IdentityService identityServicw,
    UserManager<IdentityUser> _userManager) : IRequestHandler<RegisterIdentityUserCommand, OperationResult<string>>
{
    private readonly IdentityService _identityService = identityServicw;
    private OperationResult<string> _result = new();
    private readonly IdentityErrorMessages _errorMessages = new();

    public async Task<OperationResult<string>> Handle(RegisterIdentityUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await ValidateIdentityDoesNotExist(request);
            if (_result.IsError) return _result;

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var identity = await CreateIdentityUserAsync(request, transaction, cancellationToken);
            if (_result.IsError) return _result;

            var profile = await CreateUserProfileAsync(identity, request, transaction, cancellationToken);
            if (_result.IsError) return _result;

           transaction.Commit();

            _result.Payload = GetJwtString(identity, profile);
            return _result;
        }

        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError, $"{ex.Message}"));
        }
        catch (Exception e)
        {
            _result.AddUnknownError($"{e.Message}");
        }
        return _result;
    }

    private async Task ValidateIdentityDoesNotExist(RegisterIdentityUserCommand request)
    {
        IdentityUser existingIdentity = await _userManager.FindByEmailAsync(request.Username);

        if (existingIdentity is not null)
        {
            _result.AddError(ErrorCode.NotFound, _errorMessages.IdentityUserAlreadyExists);

        }

        
    }

    private async Task<IdentityUser> CreateIdentityUserAsync( 
        RegisterIdentityUserCommand request, IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        var newIdentity = new IdentityUser(){ Email = request.Username, UserName = request.Username};
        var createdIdentityUser = await _userManager.CreateAsync(newIdentity, request.Password);
        if (!createdIdentityUser.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var identityError in createdIdentityUser.Errors)
                _result.AddError(ErrorCode.IdentityCreationFailed, identityError.Description);
            
        }
        return newIdentity;
    }

    private async Task<UserProfile> CreateUserProfileAsync(IdentityUser identity,
        RegisterIdentityUserCommand request, IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            var profileInfo = BasicInfo.CreateBasicInfo(
                request.FirstName,
                request.LastName,
                request.Username,
                request.Phone,
                request.DateOfBirth,
                request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
         
            _context.UserProfiles.Add(profile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return profile;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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
        return _identityService.WriteToken(token);
    }
}
