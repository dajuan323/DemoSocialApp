namespace DemoSocial.Application.Identity;

public record IdentityErrorMessages
{
    public string NonExistentIdentityUser = "Unable to find user. Login failed.";

    public string IncorrectPassword = "Password incorrect. Login failed.";

    public string IdentityUserAlreadyExists = "Provided email address already exists.  Cannot register new user.";

    public string CreateTokenFailure = "Failed to create token for user.";

    public string UnauthorizedAccountRemoval = "Account removal unauthorized.";
}
