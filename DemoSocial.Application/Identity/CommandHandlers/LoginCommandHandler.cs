using AutoMapper;
using Azure.Core;
using DemoSocial.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.Identity.CommandHandlers;

internal class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private OperationResult<string> _result = new();
    private readonly IdentityErrorMessages _errorMessages = new();

    public LoginCommandHandler(DataContext context, UserManager<IdentityUser> userManager,  IdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
    }

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
