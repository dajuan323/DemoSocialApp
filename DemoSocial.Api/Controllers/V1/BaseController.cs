using Azure;
using DemoSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DemoSocial.Api.Controllers.V1;

public class BaseController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        var apiError = new ErrorResponse();

        if (errors.Any(e => e.Code == ErrorCode.NotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }


        apiError.StatusCode = 400;
        apiError.StatusPhrase = "Bad Request";
        apiError.Timestamp = DateTime.Now;
        apiError.Errors.Add("Unknown Error");
        errors.ForEach(e => apiError.Errors.Add(e.Message));
        return StatusCode(500, apiError);
        


    }
}
