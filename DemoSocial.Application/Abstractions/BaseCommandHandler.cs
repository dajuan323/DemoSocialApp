using DemoSocial.Application.Enums;
using DemoSocial.Application.Models;
using MediatR;

namespace DemoSocial.Application.Abstractions;

internal class BaseCommandHandler 
{
    protected static OperationResult<string> ElxceptionErrorHandler(OperationResult<string> result, ErrorCode code, string message)
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
