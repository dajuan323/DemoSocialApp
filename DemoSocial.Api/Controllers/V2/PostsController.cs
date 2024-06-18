using Asp.Versioning;
using DemoSocial.Domain.TestModels;
using Microsoft.AspNetCore.Mvc;

namespace DemoSocial.Api.Controllers.V2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{

    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var post = new TestPost { Id = id, Text = "Hello, Universe!" };
        return Ok(post);
    }
}
