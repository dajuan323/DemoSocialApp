namespace SharedKernel;

public record Error
{
    public ErrorCode Code { get; set; }
    public string? Message { get; set; }
};

