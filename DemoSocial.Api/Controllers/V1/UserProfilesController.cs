namespace DemoSocial.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfilesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserProfilesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllUserProfiles();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(profiles);
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


}

