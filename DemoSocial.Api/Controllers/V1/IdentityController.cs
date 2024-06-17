namespace DemoSocial.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
public class IdentityController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public IdentityController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }


    [HttpPost]
    [Route(ApiRoutes.Identiy.Registration)]
    [ValidateModel]
    public async Task<IActionResult> RegisterIdUser([FromBody] UserRegistrationContract newUser)
    {
        var command = _mapper.Map<RegisterIdentityUserCommand>(newUser);
        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        AuthenticationResult authenticationResult = new() {Token = result.Payload };

        return Ok(authenticationResult);
    }

    [HttpPost]
    [Route(ApiRoutes.Identiy.Login)]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginContract login)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var result = await _mediator.Send(command);

        if (result.IsError) return HandleErrorResponse(result.Errors);

        AuthenticationResult authenticationResult = new() { Token = result.Payload };

        return Ok(authenticationResult);
    }



}
