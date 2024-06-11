namespace DemoSocial.Api.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings() 
    {
        CreateMap<UserRegistrationContract, RegisterIdentityUserCommand>();
    }
}
