using DemoSocial.Domain.Aggregates.UserProfileAggregate;

namespace DemoSocial.Api.Contracts.UserProfile.Responses;

public record UserProfileResponse
{
    public Guid UserProfileId { get; set; }
    public BasicInfo BasicInfo { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastModified { get; set; }
}
