using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.Commands;

public class DeleteUserProfileCommad : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}

