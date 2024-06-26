﻿using DemoSocial.Application.UserProfiles.Queries;
using DemoSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoSocial.Application.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfilesQueryHandler(
        IDataContext context) : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<UserProfile>>>
    {
        private readonly IDataContext _context = context;
        private OperationResult<IEnumerable<UserProfile>> _result = new();
        private readonly UserProfileErrorMessages _errorMessages = new();

        public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<UserProfile> profiles = await _context.UserProfiles.ToListAsync(cancellationToken: cancellationToken);
                if (profiles is null)
                {
                    _result.AddError(ErrorCode.NotFound, _errorMessages.UserProfileNotFound);
                  
                }
                _result.Payload = profiles.ToArray();
            }
            catch (Exception ex)
            {
                _result.AddUnknownError($"{ex.Message}");
            }
            return _result;
            
        }
    }
}
