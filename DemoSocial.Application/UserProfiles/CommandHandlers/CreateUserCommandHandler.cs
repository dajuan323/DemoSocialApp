using AutoMapper;
using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Domain.Exceptions;
using MediatR;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.CommandHandlers;

internal class CreateUserCommandHandler(
    IDataContext context,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
{
    private readonly IDataContext _context = context;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private OperationResult<UserProfile> _result = new();
    private readonly UserProfileErrorMessages _errorMessages = new();

    public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var basicInfo = BasicInfo.CreateBasicInfo(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.DateOfBirth,
            request.CurrentCity);

            var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

            _context.UserProfiles.Add(userProfile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _result.Payload = userProfile;
        }

        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError, e)); ;
        }

        catch (Exception e)
        {
            _result.AddUnknownError($"{e.Message}");
        }

        return _result;
    }
}


