using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using DemoSocial.Domain.Exceptions;
using DemoSocial.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoCommandHandler(DataContext context) : IRequestHandler<UpdateUserProfileBasicInfo, OperationResult<UserProfile>>
    {
        private readonly DataContext _context = context;

        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();

            try
            {
                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

                if (userProfile == null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No User Profile found with Id {request.UserProfileId}"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var basicInfo = BasicInfo.CreateBasicInfo(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Phone,
                    request.DateOfBirth,
                    request.CurrentCity);

                userProfile.UpdateBasicInfo(basicInfo);

                _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = userProfile;
                return result;
            }

            catch (UserProfileNotValidException ex)
            {

                result.IsError = true;
                ex.ValidationErrors.ForEach(e =>
                {
                    Error error = new()
                    {
                        Code = ErrorCode.ValidationError,
                        Message = $"{ex.Message}"

                    };
                    result.Errors.Add(error);

                });

                return result;
            }

            catch (Exception e)
            {

                result.IsError = true;
                var error = new Error
                {
                    Code = ErrorCode.ServerError,
                    Message = e.Message
                };
                result.Errors.Add(error);
            }



            return result;
        }
    }
}
