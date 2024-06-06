﻿using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.Queries;

public class GetUserProfileByIdQuery : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
