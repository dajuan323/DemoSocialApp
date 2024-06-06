using Asp.Versioning;
using AutoMapper;
using DemoSocial.Api.Contracts.Common;
using DemoSocial.Api.Contracts.UserProfile.Requests;
using DemoSocial.Api.Contracts.UserProfile.Responses;
using DemoSocial.Api.Filters;
using DemoSocial.Application.Enums;
using DemoSocial.Application.UserProfiles.Commands;
using DemoSocial.Application.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemoSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfilesController(IMediator mediator, IMapper mapper) : BaseController
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    

    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        //throw new NotImplementedException("Method not implemented.");

        var query = new GetAllUserProfiles();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(profiles);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<CreateUserCommand>(profile);
        var response = await _mediator.Send(command);
        var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);

        return response.IsError ? HandleErrorResponse(response.Errors) : CreatedAtAction(nameof(GetUserProfileById),
            new {id=userProfile.UserProfileId}, userProfile);
    }

    [Route(ApiRoutes.UserProfiles.Idroute)]
    [HttpGet]
    [ValidateGuid("id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileByIdQuery { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);

        return (response.IsError) ?
            HandleErrorResponse(response.Errors) : Ok(userProfile);


    }

    [HttpPatch]
    [Route(ApiRoutes.UserProfiles.Idroute)]
    [ValidateModel]
    [ValidateGuid("id")]
    public async Task<IActionResult> UpdateProfile(string id, UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<UpdateUserProfileBasicInfo>(profile);
        command.UserProfileId = Guid.Parse(id);
        var response = await _mediator.Send(command);

        return (response.IsError) ? 
            HandleErrorResponse(response.Errors) : 
            NoContent();

    }

    [HttpDelete]
    [Route(ApiRoutes.UserProfiles.Idroute)]
    [ValidateGuid("id")]
    public async Task<IActionResult> DeleteUserProfile(string id)
    {
        var command = new DeleteUserProfileCommad() { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(command);

        return (response.IsError) ?
            HandleErrorResponse(response.Errors) :
            NoContent();
    }

}

