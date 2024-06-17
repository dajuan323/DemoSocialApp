using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharedKernel;

public class SharedErrorHandler
{
    public static OperationResult<T> BaseExceptionErrorHandler<T>(OperationResult<T> result, ErrorCode code, string message)
    {
        result.IsError = true;
        Error error = new()
        {
            Code = code,
            Message = message
        };
        result.Errors.Add(error);
        return result;
    }
}
