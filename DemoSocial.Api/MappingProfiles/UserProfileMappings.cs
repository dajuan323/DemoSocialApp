namespace DemoSocial.Api.MappingProfiles;

public class UserProfileMappings : Profile
{
    public UserProfileMappings()
    {
        CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
        CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfo>();
        CreateMap<UserProfile, UserProfileResponse>();
        CreateMap<BasicInfo, BasicInfoResponse>();
    }
}
