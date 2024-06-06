using DemoSocial.Application.Models;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.Commands;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    DateTime DateOfBirth,
    string CurrentCity) : IRequest<OperationResult<UserProfile>>;

