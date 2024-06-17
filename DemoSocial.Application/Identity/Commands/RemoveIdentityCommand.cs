namespace DemoSocial.Application.Identity.Commands;

public sealed record RemoveIdentityCommand(
    Guid IdentityUserId,
    Guid RequestorGuid) : IRequest<OperationResult<bool>>;
