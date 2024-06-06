using DemoSocial.Api.Contracts.Common;
using DemoSocial.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DemoSocial.Api.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            ErrorResponse apiError = new()
            {
                StatusCode = 400,
                StatusPhrase = "Bad Request",
                Timestamp = DateTime.Now
            };
            var errors = context.ModelState.AsEnumerable();

            foreach (var error in errors)
            {
                foreach (var innerValue in error.Value.Errors)
                {
                    apiError.Errors.Add(innerValue.ErrorMessage);
                }
            }

            context.Result = new BadRequestObjectResult(apiError);

            // TO DO: Make sure Asp.NET Core doesnt override action result body
        }
    }
}
