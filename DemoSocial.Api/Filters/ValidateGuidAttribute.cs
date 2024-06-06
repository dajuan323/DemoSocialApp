using DemoSocial.Api.Contracts.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoSocial.Api.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly string _key;

    public ValidateGuidAttribute(string key) => _key = key;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.TryGetValue(_key, out var value) ||
                Guid.TryParse(value?.ToString(), out Guid guid)) return;

        var apiError = new ErrorResponse
        {
            StatusCode = 400,
            StatusPhrase = "Bad Request",
            Timestamp = DateTime.Now,
            Errors = { $"The identifier for {_key} is not in correct GUID format" }
        };

        context.Result = new ObjectResult(apiError);



    }
}
