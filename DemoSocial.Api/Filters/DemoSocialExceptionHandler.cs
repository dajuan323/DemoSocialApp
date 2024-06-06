using DemoSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoSocial.Api.Filters;

public class DemoSocialExceptionHandler : ExceptionFilterAttribute
{

    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse
        {
            StatusCode = 404,
            StatusPhrase = "Internal Server Error",
            Timestamp = DateTime.Now,
            Errors = {context.Exception.Message}
        };

        context.Result = new JsonResult(apiError) { StatusCode = apiError.StatusCode};
    }
}
