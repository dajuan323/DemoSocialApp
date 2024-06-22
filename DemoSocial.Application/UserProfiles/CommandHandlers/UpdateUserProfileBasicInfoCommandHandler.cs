using DemoSocial.Application.UserProfiles.Commands;
using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace DemoSocial.Application.UserProfiles.CommandHandlers
{
    internal class UpdateUserProfileBasicInfoCommandHandler(
        IDataContext context,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserProfileBasicInfo, OperationResult<UserProfile>>
    {
        private readonly IDataContext _context = context;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private OperationResult<UserProfile> _result;

        public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfo request, CancellationToken cancellationToken)
        {
            try
            {
                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken: cancellationToken);

                if (userProfile == null)
                {
                    _result.AddError(ErrorCode.NotFound, "replace this");
                    return _result;
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
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _result.Payload = userProfile;
            }

            catch (UserProfileNotValidException ex)
            {
                ex.ValidationErrors.ForEach(e => _result.AddError(ErrorCode.ValidationError, $"{ex.Message}"));
            }

            catch (Exception e)
            {
                _result.AddUnknownError($"{e.Message}");
            }
            return _result;
        }
    }
}
