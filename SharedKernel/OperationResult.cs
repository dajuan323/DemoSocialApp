
namespace SharedKernel;

public class OperationResult<T>
{
    public T Payload { get; set; }
    public bool IsError { get; set; }
    public List<Error> Errors { get; set; } = [];

    /// <summary>
    /// Adds a default error to the Error list with the error code Unknown
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void AddError(ErrorCode code, string message)
    {
        HandleError(code, message);
    }

    public void AddUnknownError(string message)
    {
        HandleError(ErrorCode.UnknownError, message);
    }

    public void ResetIsErrorFlag()
    {
        IsError = false;
    }

    private void HandleError(ErrorCode code, string message)
    {
        Errors.Add(new Error { Code = code, Message = message }); 
        IsError = true;
    }
}