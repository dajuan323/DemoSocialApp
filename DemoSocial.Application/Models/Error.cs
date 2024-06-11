namespace DemoSocial.Application.Models;

public record Error
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }
};

