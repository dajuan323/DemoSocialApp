namespace DemoSocial.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
public class IdentityController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator) => _mediator = mediator;
    public IdentityController(Mapper mapper) => _mapper = mapper;

    [HttpPost]
    [Route(ApiRoutes.Identiy.Registration)]
    [ValidateModel]
    public async Task<IActionResult> RegisterIdUser([FromBody] UserRegistrationContract newUser)
    {
        var command = _mapper.Map<RegisterIdentityUserCommand>(newUser);
        var result = await _mediator.Send(command);

        if (result.IsError) HandleErrorResponse(result.Errors);

        AuthenticationResult authenticationResult = new() {Token = result.Payload };

        return Ok(authenticationResult);
    }
}
