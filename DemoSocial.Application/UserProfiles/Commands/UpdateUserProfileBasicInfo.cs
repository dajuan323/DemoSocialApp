using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using SharedKernel;


namespace DemoSocial.Application.UserProfiles.Commands;

public class UpdateUserProfileBasicInfo : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}
