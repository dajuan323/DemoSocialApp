namespace DemoSocial.Application.Identity.Commands;

public sealed record RegisterIdentityUserCommand(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Phone,
    string CurrentCity) : IRequest<OperationResult<string>>;

