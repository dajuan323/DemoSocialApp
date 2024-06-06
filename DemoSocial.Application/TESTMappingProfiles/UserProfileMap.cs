using AutoMapper;
using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.TESTMappingProfiles;

internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        CreateMap<CreateUserCommand, BasicInfo>();
    }
}
