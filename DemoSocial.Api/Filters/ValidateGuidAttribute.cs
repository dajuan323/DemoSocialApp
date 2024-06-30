namespace DemoSocial.Api.Filters;

public class ValidateGuidAttribute : ActionFilterAttribute
{
    private readonly List<string> _keys;

    public ValidateGuidAttribute(string key)
    {
        _keys = new List<string>();
        _keys.Add(key);
    }

    public ValidateGuidAttribute(string key1, string key2)
    {
        _keys = new List<string>();
        _keys.Add(key1);
        _keys.Add(key2);
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        bool hasError = false;
        var apiError = new ErrorResponse();
        _keys.ForEach(k =>
        {

            if (!context.ActionArguments.TryGetValue(k, out var value) ||
                !Guid.TryParse(value?.ToString(), out Guid guid))
            {
                hasError = true;
                apiError.Errors.Add($"The identifier for {k} is not in correct GUID format");

            }



        });

        if (hasError)
        {

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            

            context.Result = new ObjectResult(apiError);
        }


    }
}
