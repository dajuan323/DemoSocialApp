using AutoMapper;
using DemoSocial.Api.Contracts.UserProfile.Requests;
using DemoSocial.Api.Contracts.UserProfile.Responses;
using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;

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
