namespace DemoSocial.Application.Identity.CommandHandlers;

internal class RegisterIdentityUserCommandHandler : BaseCommandHandler, IRequestHandler<RegisterIdentityUserCommand, OperationResult<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public RegisterIdentityUserCommandHandler(DataContext context, UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
    {
        _context = context;
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }

    public async Task<OperationResult<string>> Handle(RegisterIdentityUserCommand request, CancellationToken cancellationToken)
    {
        OperationResult<string> result = new();
        try
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

            if (existingIdentity is null)
            {                
                ElxceptionErrorHandler(
                        result, 
                        ErrorCode.IdentityUserAlreadyExists,
                        $"Provided email address already exists.  Cannot register new user."
                    );
            }

            IdentityUser identity = new()
            {
                Email = request.Username,
                UserName = request.Username,
            };


            using var transaction = _context.Database.BeginTransaction();
            var createdIdentity = await _userManager.CreateAsync(existingIdentity, request.Password);
            if (!createdIdentity.Succeeded)
            {
                await transaction.RollbackAsync();
                foreach (var identityError in createdIdentity.Errors)
                {
                    ElxceptionErrorHandler(
                    result,
                    ErrorCode.IdentityCreationFailed,
                    identityError.Description);
                }

            }

            var profileInfo = BasicInfo.CreateBasicInfo(
                request.FirstName,
                request.LastName,
                request.Username,
                request.Phone,
                request.DateOfBirth,
                request.CurrentCity);

            var profile = UserProfile.CreateUserProfile(identity.Id, profileInfo);

            try
            {
                _context.UserProfiles.Add(profile);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                
            }
            catch (Exception)
            {
                await transaction.RollbackAsync( );
                throw;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: identity.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString(ToString())),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: identity.Email),
                    new Claim(type: "IdentityId", value: identity.Id),
                    new Claim(type: "UserProfileId", value: profile.UserProfileId.ToString())
                }),
                Expires = DateTime.Now.AddHours(2),
                Audience = _jwtSettings.Audiences[0],
                Issuer = _jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            result.Payload  = tokenHandler.WriteToken(token);
            return result;
        }
        
        catch (UserProfileNotValidException ex)
        {
            ElxceptionErrorHandler(result, ErrorCode.ValidationError, $"{ex.Message}" );
        }
        catch (Exception e)
        {          
            ElxceptionErrorHandler(result, ErrorCode.UnknownError, $"{e.Message}");
        }

        return result;

    }

}
